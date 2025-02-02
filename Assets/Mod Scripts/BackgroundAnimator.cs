using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Rnd = UnityEngine.Random;

public class BackgroundAnimator : MonoBehaviour
{
    public Image BackingColour;
    public Image RingTemplate;
    public Image SpiralA;
    public Image SpiralB;
    public int RingEndSize;

    private List<Image> Rings;
    private Color CurrentColour;
    private Color DarkColour;

    // Use this for initialization
    void Start()
    {
        Rings = new List<Image>();
        RunSimulation();
        StartCoroutine(RotateSpirals());
        RingTemplate.gameObject.SetActive(false);
    }

    public void SetColour(Blind blind)
    {
        var name = blind.GetName();
        var colour = blind.GetBackgroundColour();
        CurrentColour = colour;
        DarkColour = Utility.FindDarkColour(CurrentColour);
        BackingColour.color = CurrentColour;
        SpiralA.color = SpiralB.color = DarkColour * new Color(1, 1, 1, SpiralA.color.a);
        foreach (Image ring in Rings.Where(x => x != null))  // This happens before garbage collection, so I have to make sure I'm not using destroyed objects.
            ring.color = DarkColour * new Color(1, 1, 1, ring.color.a);
    }

    private Image CreateRing()
    {
        var ring = Instantiate(RingTemplate, RingTemplate.transform.parent);
        ring.gameObject.SetActive(true);
        ring.rectTransform.sizeDelta = Vector2.zero;
        ring.color = new Color(DarkColour.r, DarkColour.g, DarkColour.b, Rnd.Range(0.1f, 0.2f));
        Rings.Add(ring);
        return ring;
    }

    private void DestroyRing(Image ring)
    {
        Rings.Remove(ring);
        Destroy(ring.gameObject);
    }

    private void RunSimulation()
    {
        float time = 0;
        while (time < 10)
        {
            RunRingSimulation(CreateRing(), time, GenerateRandomDuration());
            time += GenerateRandomInterval();
        }
        StartCoroutine(RunAnimation());
    }

    void RunRingSimulation(Image ring, float time, float duration)
    {
        Rings.Add(ring);
        if (time + duration <= 10)
            DestroyRing(ring);
        ring.rectTransform.sizeDelta = Vector2.one * Easing.InSine(10 - time, 0, RingEndSize, duration);
        StartCoroutine(RunPartialRingCycle(ring, 10 - time, duration));
    }

    private IEnumerator RunAnimation()
    {
        while (true)
        {
            StartCoroutine(RunRingCycle(CreateRing(), GenerateRandomDuration()));
            yield return new WaitForSeconds(GenerateRandomInterval());
        }
    }

    private float GenerateRandomDuration()
    {
        return Rnd.Range(8f, 10f);
    }

    private float GenerateRandomInterval()
    {
        return Rnd.Range(0.2f, 0.3f);
    }

    private IEnumerator RunRingCycle(Image ring, float duration)
    {
        Rings.Add(ring);
        float timer = 0;
        while (timer < duration)
        {
            ring.rectTransform.sizeDelta = Vector2.one * Easing.InSine(timer, 0, RingEndSize, duration);
            yield return null;
            timer += Time.deltaTime;
        }
        DestroyRing(ring);
    }

    private IEnumerator RunPartialRingCycle(Image ring, float timer, float duration)
    {
        while (timer < duration)
        {
            ring.rectTransform.sizeDelta = Vector2.one * Easing.InSine(timer, 0, RingEndSize, duration);
            yield return null;
            timer += Time.deltaTime;
        }
        DestroyRing(ring);
    }

    private IEnumerator RotateSpirals(float duration = 10)
    {
        while (true)
        {
            yield return null;
            SpiralA.transform.localEulerAngles = Vector3.forward * Mathf.Lerp(360, 0, (Time.time / duration) % 1);
            SpiralB.transform.localEulerAngles = Vector3.forward * Mathf.Lerp(0, 360, (Time.time / duration) % 1);
        }
    }
}
