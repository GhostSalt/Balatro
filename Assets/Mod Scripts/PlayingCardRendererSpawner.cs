using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardRendererSpawner : MonoBehaviour
{
    public Sprite[] AllBaseSprites;
    public Sprite[] AllFaceSprites;
    public SpriteRenderer BaseTemplate;
    public SpriteRenderer FaceTemplate;

    void Start()
    {
        BaseTemplate.gameObject.SetActive(false);
        FaceTemplate.gameObject.SetActive(false);
    }

    public PlayingCardRenderer SpawnCard(PlayingCard card, CardArea area)
    {
        var obj = new GameObject(card.GetName());
        obj.transform.parent = area.transform;
        var rend = obj.AddComponent<PlayingCardRenderer>();
        rend.Initialise(new Card(card.GetName()), FindBaseSprite(card.GetEnhancement()), FindFaceSprite(card.GetName()), BaseTemplate, FaceTemplate);
        return rend;
    }

    private Sprite FindBaseSprite(string enhancement)
    {
        foreach (Sprite candidate in AllBaseSprites)
            if (candidate.name == enhancement + " card")
                return candidate;
        throw new System.Exception("Couldn't find the card enhancement: \"" + enhancement + "\"!");
    }

    private Sprite FindFaceSprite(string name)
    {
        foreach (Sprite candidate in AllFaceSprites)
            if (candidate.name == name)
                return candidate;
        throw new System.Exception("Couldn't find the card face: \"" + name + "\"!");
    }
}
