using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColouredUISelectable : UISelectable
{
    private Color Colour;

    public void SetColour(Color colour)
    {
        Colour = Rend.color = colour;
    }
}
