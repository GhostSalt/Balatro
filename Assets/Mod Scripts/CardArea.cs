using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArea : MonoBehaviour
{
    public float Width;

    private CardRenderer[] Cards;

    public void SetCards(CardRenderer[] cards)
    {
        Cards = cards;
        ArrangeCards();
    }

    public void ArrangeCards()
    {
        for (int i = 0; i < Cards.Length; i++)
            Cards[i].SetLocation(FindCardLocation(i, Cards.Length));
    }

    private Vector3 FindCardLocation(int pos, int count)
    {
        if (count == 1)
            return Vector3.zero;
        return Vector3.right * Mathf.Lerp(transform.localPosition.x - Width, transform.localPosition.x + Width, pos / (count - 1f));
    }
}
