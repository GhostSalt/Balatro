using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCard : Card
{
    private string Enhancement;

    public PlayingCard(string name, string enhancement) : base(name)
    {
        Enhancement = enhancement;
    }

    public string GetEnhancement()
    {
        return Enhancement;
    }

    public override string ToString()
    {
        return GetName() + " (" + Enhancement + ")";
    }
}
