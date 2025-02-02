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

    public CardRenderer SpawnCard(string name, CardArea area)
    {
        var obj = new GameObject(name);
        obj.transform.parent = area.transform;
        var rend = obj.AddComponent<CardRenderer>();
        rend.Initialise(new Card(name), FindCardSprite(name), FindSoulSprite(name), CardTemplate, SoulTemplate);

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
