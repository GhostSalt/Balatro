using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour
{
    private Card InternalCard;
    private SpriteRenderer BaseRend;
    private SpriteRenderer SoulRend;

    private const float POSITION_INNER = 0.025f;
    private const float POSITION_OUTER = 0.075f;

    public void Initialise(Card internalCard, Sprite baseSprite, Sprite soulSprite, SpriteRenderer baseTemplate, SpriteRenderer soulTemplate)
    {
        InternalCard = internalCard;

        BaseRend = CreateRend(baseTemplate);
        BaseRend.sprite = baseSprite;

        if (soulSprite != null)
        {
            SoulRend = CreateRend(soulTemplate);
            SoulRend.sprite = soulSprite;
            SoulRend.transform.localPosition = Vector3.back * POSITION_INNER;
            SoulRend.sortingOrder = 1;
            StartCoroutine(AnimateSoul());
        }
        StartCoroutine(Animate());
    }

    private SpriteRenderer CreateRend(SpriteRenderer template)
    {
        var rend = Instantiate(template, transform);
        rend.gameObject.SetActive(true);
        transform.localScale = Vector3.one * 0.35f;
        return rend;
    }

    public void SetLocation(Vector3 location)
    {
        transform.localPosition = location;
    }

    private IEnumerator Animate(float angleBoundary = 8f, float speedX = Mathf.PI / 8f / 5f, float speedZ = 2.718281828459045235360287471352f / 5f / 5f)
    {
        while (true)
        {
            var x = Mathf.Lerp(-angleBoundary, angleBoundary, (Mathf.Sin(Mathf.PI * 2 * ((Time.time + transform.localPosition.x + transform.localPosition.y) * speedX % 1)) + 1) / 2);
            var z = Mathf.Lerp(-angleBoundary, angleBoundary, (Mathf.Sin(Mathf.PI * 2 * ((Time.time + transform.localPosition.x + transform.localPosition.y) * speedZ % 1)) + 1) / 2);
            transform.localRotation = Quaternion.Euler(z, x, 0);
            yield return null;
        }
    }

    private IEnumerator AnimateSoul(float angleBoundary = 10f, float scaleLower = 0.95f, float scaleUpper = 1.1f, float speedR = Mathf.PI / 25f, float speedS = 2.718281828459045235360287471352f / 15f)
    {
        while (true)
        {
            var t = (Time.time + transform.localPosition.x + transform.localPosition.y);
            var r = Mathf.Lerp(-angleBoundary, angleBoundary, (Mathf.Sin(Mathf.PI * 2 * (t * speedR % 1)) + 1) / 2);
            var s = Mathf.Lerp(scaleLower, scaleUpper, (Mathf.Sin(Mathf.PI * 2 * (t * speedS % 1)) + 1) / 2);
            var p = Mathf.Lerp(POSITION_INNER, POSITION_OUTER, (Mathf.Sin(Mathf.PI * 2 * (t * speedS % 1)) + 1) / 2);
            SoulRend.transform.localRotation = Quaternion.Euler(0, 0, r);
            SoulRend.transform.localScale = Vector3.one * s;
            SoulRend.transform.localPosition = Vector3.back * p;
            yield return null;
        }
    }
}
