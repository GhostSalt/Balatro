using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rnd = UnityEngine.Random;

public class BackgroundAnimator : MonoBehaviour
{
    public Image RingTemplate;
    public Image SpiralA;
    public Image SpiralB;
    public int RingEndSize;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(RunAnimation());
        StartCoroutine(RotateSpirals());
        RingTemplate.gameObject.SetActive(false);
    }

    private Image CreateRing()
    {
        var ring = Instantiate(RingTemplate, RingTemplate.transform.parent);
        ring.gameObject.SetActive(true);
        ring.rectTransform.sizeDelta = Vector2.zero;
        ring.color = new Color(ring.color.r, ring.color.g, ring.color.b, Rnd.Range(0.25f, 0.5f));
        return ring;
    }

    private void DestroyRing(Image ring)
    {
        Destroy(ring.gameObject);
    }

    private IEnumerator RunAnimation()
    {
        while (true)
        {
            StartCoroutine(RunRingCycle(CreateRing(), Rnd.Range(8f, 10f)));
            yield return new WaitForSeconds(Rnd.Range(0.2f, 0.3f));
        }
    }

    private IEnumerator RunRingCycle(Image ring, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            ring.rectTransform.sizeDelta = Vector2.one * Mathf.Lerp(0, RingEndSize, timer / duration);
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
