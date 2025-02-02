using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRendererSpawner : MonoBehaviour
{
    public Sprite[] AllCardSprites;
    public SpriteRenderer CardTemplate;
    public SpriteRenderer SoulTemplate;

    void Start()
    {
        CardTemplate.gameObject.SetActive(false);
        SoulTemplate.gameObject.SetActive(false);
    }

    public CardRenderer SpawnCard(Card card, CardArea area)
    {
        var obj = new GameObject(card.GetName());
        obj.transform.parent = area.transform;
        var rend = obj.AddComponent<CardRenderer>();
        rend.Initialise(new Card(card.GetName()), FindCardSprite(card.GetName()), FindSoulSprite(card.GetName()), CardTemplate, SoulTemplate);

        return rend;
    }

    private Sprite FindCardSprite(string name)
    {
        foreach (Sprite candidate in AllCardSprites)
            if (candidate.name == name)
                return candidate;
        throw new System.Exception("Couldn't find the card: \"" + name + "\"!");
    }

    private Sprite FindSoulSprite(string name)
    {
        foreach (Sprite candidate in AllCardSprites)
            if (candidate.name == name + " soul")
                return candidate;
        return null;
    }
}
