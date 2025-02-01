using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private string Name;

    public Card(string name)
    {
        Name = name;
    }

    public string GetName()
    {
        return Name;
    }

    public override string ToString()
    {
        return Name;
    }
}