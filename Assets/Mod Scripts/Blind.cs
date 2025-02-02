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
    }

    public string GetName()
    {
        return Name;
    }

    public Color GetColour()
    {
        return Colour;
    }

    public override string ToString()
    {
        return Name;
    }
}