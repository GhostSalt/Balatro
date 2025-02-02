using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardRenderer : CardRenderer
{
    protected SpriteRenderer FaceRend;

    public override void Initialise(Card internalCard, Sprite baseSprite, Sprite faceSprite, SpriteRenderer baseTemplate, SpriteRenderer faceTemplate)
    {
        base.Initialise(internalCard, baseSprite, null, baseTemplate, null);

        FaceRend = CreateRend(faceTemplate);
        FaceRend.sprite = faceSprite;
        FaceRend.transform.localPosition = Vector3.zero;
    }

    public override void SetZ(int z)
    {
        base.SetZ(z);
        SetFaceZ(z);
    }

    private void SetFaceZ(int z)
    {
        if (FaceRend != null)
            FaceRend.sortingOrder = (z * 10) + 1;
    }

    protected override IEnumerator DestroyLogic()
    {
        yield return StartCoroutine(DisappearAnim(NATIVE_SCALE));
        Destroy(BaseRend.gameObject);
        Destroy(FaceRend.gameObject);
        Destroy(gameObject);
    }
}
