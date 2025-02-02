using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind
{
    private string Name;
    private Color Colour;

    public Blind(string name)
    {
        Name = name;
        Colour = BlindData.FindBlindColour(name);
    }

    public string GetName()
    {
        return Name;
    }

    public Color GetColour()
    {
        return Colour;
    }

    public Color GetBackgroundColour()
    {
        if (Name != "small blind" && Name != "big blind")
            return Colour;
        return new Color32(64, 128, 102, 255);
    }

    public override string ToString()
    {
        return Name;
    }
}