using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardArea : MonoBehaviour
{
    public Text Text;
    public float Width;
    [UnityEngine.SerializeField]
    private int Capacity;

    private CardRenderer[] Cards;

    public void SetCards(CardRenderer[] cards)
    {
        Cards = cards;
        UpdateText();
        ArrangeCards();
    }

    public void ArrangeCards()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].SetLocation(FindCardLocation(i, Cards.Length));
            Cards[i].SetZ(i);
        }
    }

    public void Activate()
    {
        for (int i = 0; i < Cards.Length; i++)
            Cards[i].Appear();
    }

    public void Deactivate()
    {
        for (int i = 0; i < Cards.Length; i++)
            Cards[i].Disappear();
    }

    private Vector3 FindCardLocation(int pos, int count)
    {
        if (count == 1)
            return Vector3.zero;
        return Vector3.right * Mathf.Lerp(transform.localPosition.x - Width, transform.localPosition.x + Width, pos / (count - 1f));
    }

    public void SetCapacity(int capacity)
    {
        Capacity = capacity;
        UpdateText();
    }

    private void UpdateText()
    {
        Text.text = Cards.Length + "/" + Capacity;
    }
}
