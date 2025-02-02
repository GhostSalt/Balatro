using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipRenderer : FloatableRenderer
{
    private Blind InternalBlind;

    public void Initialise(Blind internalBlind, Sprite[] baseSprites, SpriteRenderer baseTemplate)
    {
        base.Initialise(baseSprites[0], baseTemplate);

        InternalBlind = internalBlind;
        StartCoroutine(Shine(baseSprites));
    }

    private IEnumerator Shine(Sprite[] sprites, float speed = 0.1f)
    {
        while (true)
            for (int i = 0; i < sprites.Length; i++)
            {
                SetBaseSprite(sprites[i]);
                yield return new WaitForSeconds(speed);
            }
    }

    public Blind GetInternalBlind()
    {
        return InternalBlind;
    }
}
