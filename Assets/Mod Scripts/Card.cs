using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private string Name;
    private Vector2 BaseSpriteCoords;
    private Vector2 SoulSpriteCoords;

    public Card(string name, Vector2 baseSpriteCoords, Vector2 soulSpriteCoords)
    {
        Name = name;
        BaseSpriteCoords = baseSpriteCoords;
        SoulSpriteCoords = soulSpriteCoords;
    }

    public string GetName()
    {
        return Name;
    }

    public Vector2 GetBaseSpriteCoords()
    {
        return BaseSpriteCoords;
    }

    public Vector2 GetSoulSpriteCoords()
    {
        return SoulSpriteCoords;
    }
}