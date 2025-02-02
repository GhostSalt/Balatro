using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : FloatableRenderer
{
    protected Card InternalCard;
    protected SpriteRenderer SoulRend;

    protected const float POSITION_INNER = 0.025f;
    protected const float POSITION_OUTER = 0.075f;
    protected const float NATIVE_SCALE = 0.35f;

    public virtual void Initialise(Card internalCard, Sprite baseSprite, Sprite soulSprite, SpriteRenderer baseTemplate, SpriteRenderer soulTemplate)
    {
        base.Initialise(baseSprite, baseTemplate);
        InternalCard = internalCard;

        if (soulSprite != null)
        {
            SoulRend = CreateRend(soulTemplate);
            SoulRend.sprite = soulSprite;
            SoulRend.transform.localPosition = Vector3.back * POSITION_INNER;
            SoulRend.sortingOrder = 1;
            StartCoroutine(AnimateSoul());
        }
    }

    public override void SetZ(int z)
    {
        base.SetZ(z);
        SetSoulZ(z);
    }

    private void SetSoulZ(int z)
    {
        if (SoulRend != null)
            SoulRend.sortingOrder = (z * 10) + 1;
    }

    public override void Appear(float scale = 1f)
    {
        StartCoroutine(AppearAnim(NATIVE_SCALE));
    }

    public override void Disappear()
    {
        StartCoroutine(DisappearAnim(NATIVE_SCALE));
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

    protected override IEnumerator DestroyLogic()
    {
        yield return StartCoroutine(DisappearAnim(NATIVE_SCALE));
        Destroy(BaseRend.gameObject);
        Destroy(SoulRend.gameObject);
        Destroy(gameObject);
    }
}
