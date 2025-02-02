using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    public Image Background;
    public Image BackingColour;
    public Image Vignette;
    public Image Ring;
    public Image Gradient;
    public Text Nameplate;

    private Color DarkColour;
    private Color LightColour;

    private void Start()
    {
        Background.color = Color.black;
        BackingColour.color = Vignette.color = Ring.color = Gradient.color = Color.clear;
        Nameplate.color = Color.clear;
    }

    public IEnumerator Activate(ChipRenderer chipRend, KMAudio audio, float duration = 1.25f, float vignetteEndAlpha = 0.35f, float gradientInitialX = 1.2f, float gradientInitialY = 1.2f, float ringEndScale = 0.75f)
    {
        audio.PlaySoundAtTransform("blind select", transform);
        Background.color = Color.clear;
        var chipColour = BlindData.FindBlindColour(chipRend.GetInternalBlind().GetName());
        BackingColour.color = Utility.FindDarkerColour(chipColour);
        LightColour = Utility.LightenColour(chipColour, 0.5f);

        chipRend.SetLocation(Vector3.up * 0.025f);
        chipRend.Appear(2);

        Nameplate.text = BlindData.FindBlindTitle(chipRend.GetInternalBlind().GetName());

        Gradient.color = LightColour;
        Gradient.transform.localPosition = new Vector3(gradientInitialX, gradientInitialY, 0);

        float timer = 0;
        while (timer < duration)
        {
            Vignette.color = LightColour * new Color(1, 1, 1, Mathf.Lerp(1, vignetteEndAlpha, timer / duration));

            Gradient.transform.localPosition = new Vector3(Easing.InOutSine(timer, -gradientInitialX, gradientInitialX, duration), Easing.InOutSine(timer, -gradientInitialY, gradientInitialY, duration), 0);

            Ring.color = LightColour * new Color(1, 1, 1, Mathf.Lerp(0.5f, 0, timer / duration));
            Ring.transform.localScale = Vector3.one * Easing.OutExpo(timer, 0, ringEndScale, duration);

            Nameplate.color = new Color(1, 1, 1, Mathf.Min(Mathf.Lerp(0, 2, timer / duration), 1));
            Nameplate.transform.localPosition = new Vector3(Easing.OutSine(Mathf.Clamp(timer * 2, 0, duration), -0.2f, 0, duration), Nameplate.transform.localPosition.y, Nameplate.transform.localPosition.z);
            yield return null;
            timer += Time.deltaTime;
        }
        Ring.color = Color.clear;
        Vignette.color = LightColour * new Color(1, 1, 1, vignetteEndAlpha);
        Gradient.color = Color.clear;
        Nameplate.color = Color.white;
        Nameplate.transform.localPosition = new Vector3(0, Nameplate.transform.localPosition.y, Nameplate.transform.localPosition.z);

        yield return new WaitForSeconds(0.25f);
    }

    public IEnumerator Deactivate(ChipRenderer chipRend, Vector3 chipEnd, float duration = 0.25f, float vignetteStartAlpha = 0.35f)
    {
        var chipInit = chipRend.GetLocation();
        float timer = 0;
        while (timer < duration)
        {
            BackingColour.color = Color.Lerp(BackingColour.color, BackingColour.color * new Color(1, 1, 1, 0), timer / duration);
            Vignette.color = LightColour * new Color(1, 1, 1, Mathf.Lerp(vignetteStartAlpha, 0, timer / duration));
            Nameplate.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer / duration);
            Nameplate.transform.localPosition = new Vector3(Easing.OutExpo(timer, 0, 0.2f, duration), Nameplate.transform.localPosition.y, Nameplate.transform.localPosition.z);
            chipRend.SetLocation(new Vector3(Easing.BackOut(timer, chipInit.x, chipEnd.x, duration), Easing.BackOut(timer, chipInit.y, chipEnd.y, duration), Easing.BackOut(timer, chipInit.z, chipEnd.z, duration)));
            chipRend.SetScale(Mathf.Lerp(2, 1, timer / duration));
            yield return null;
            timer += Time.deltaTime;
        }
        Vignette.color = Color.clear;
        Nameplate.color = Color.clear;
        chipRend.SetLocation(chipEnd);
    }
}
