using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour
{
	private Card InternalCard;
	private MeshRenderer SpriteRend;

    public CardRenderer(Card internalCard, MeshRenderer spriteRend)
    {
        InternalCard = internalCard;
        SpriteRend = spriteRend;
    }

    public void SetLocation(Vector3 location)
    {
        SpriteRend.transform.localPosition = location;
    }

    public void Initialise()
    {

    }

    private IEnumerator Animate()
    {
        while (true) {

        }
    }
}
