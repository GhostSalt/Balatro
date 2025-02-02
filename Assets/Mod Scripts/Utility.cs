using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static Color FindDarkishColour(Color colour)
    {
        return colour * new Color(0.75f, 0.75f, 0.75f, 1f);
    }

    public static Color FindDarkColour(Color colour)
    {
        return colour * new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public static Color FindDarkerColour(Color colour)
    {
        return new Color(Mathf.Lerp(colour.r, 0, 0.4f), Mathf.Lerp(colour.g, 0, 0.4f), Mathf.Lerp(colour.b, 0, 0.4f));
    }

    public static Color LightenColour(Color colour, float lightness)
    {
        return new Color(Mathf.Lerp(colour.r, 1, lightness), Mathf.Lerp(colour.g, 1, lightness), Mathf.Lerp(colour.b, 1, lightness));
    }

    public static Color FullySaturate(Color colour)
    {
        float r = colour.r;
        float g = colour.g;
        float b = colour.b;

        float max = Mathf.Max(r, g, b);
        float min = Mathf.Min(r, g, b);
        if (max == min)
            return colour;  // Already fully-saturated

        float mid = r + g + b - max - min;

        float pos = Mathf.InverseLerp(min, max, mid);
        float newMid = Mathf.Lerp(0, max, pos);

        return new Color(r == max ? max : r == mid ? newMid : 0, g == max ? max : g == mid ? newMid : 0, b == max ? max : b == mid ? newMid : 0, colour.a);
    }
}
