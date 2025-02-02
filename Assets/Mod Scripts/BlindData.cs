using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class BlindData : MonoBehaviour
{
    private static readonly Dictionary<string, string> BlindToHex = new Dictionary<string, string>()
    {
        { "small blind", "3A55AB" },
        { "big blind", "E0A23A" },
        { "ox", "B95B08" },
        { "house", "5186A8" },
        { "club", "B9CB92" },
        { "fish", "3E85BD" },
        { "window", "A9A295" },
        { "hook", "A84024" },
        { "manacle", "575757" },
        { "wall", "8A59A5" },
        { "wheel", "50BF7C" },
        { "arm", "6865F3" },
        { "psychic", "EFC03C" },
        { "goad", "B95C96" },
        { "water", "C6E0EB" },
        { "serpent", "439A4F" },
        { "pillar", "7E6752" },
        { "eye", "4B71E4" },
        { "mouth", "AE718E" },
        { "plant", "709284" },
        { "needle", "5C6E31" },
        { "head", "AC9DB4" },
        { "tooth", "B52D2D" },
        { "mark", "6A3847" },
        { "flint", "E56A2F" },
        { "crimson heart", "AC3232" },
        { "cerulean bell", "009EFF" },
        { "amber acorn", "FDA200" },
        { "verdant leaf", "56A786" },
        { "violet vessel", "8A71E1" }
    };

    private static readonly Dictionary<string, string> BlindToTitle = new Dictionary<string, string>()
    {
        { "small blind", "Small Blind" },
        { "big blind", "Big Blind" },
        { "ox", "The Ox" },
        { "house", "The House" },
        { "club", "The Club" },
        { "fish", "The Fish" },
        { "window", "The Window" },
        { "hook", "The Hook" },
        { "manacle", "The Manacle" },
        { "wall", "The Wall" },
        { "wheel", "The Wheel" },
        { "arm", "The Arm" },
        { "psychic", "The Psychic" },
        { "goad", "The Goad" },
        { "water", "The Water" },
        { "serpent", "The Serpent" },
        { "pillar", "The Pillar" },
        { "eye", "The Eye" },
        { "mouth", "The Mouth" },
        { "plant", "The Plant" },
        { "needle", "The Needle" },
        { "head", "The Head" },
        { "tooth", "The Tooth" },
        { "mark", "The Mark" },
        { "flint", "The Flint" },
        { "crimson heart", "Crimson Heart" },
        { "cerulean bell", "Cerulean Bell" },
        { "amber acorn", "Amber Acorn" },
        { "verdant leaf", "Verdant Leaf" },
        { "violet vessel", "Violet Vessel" }
    };

    public static Color FindBlindColour(string name)
    {
        return FromHex(BlindToHex[name]);
    }

    private static Color FromHex(string hex)
    {
        if (hex.Length < 6)
            throw new System.FormatException("Hex must be at least 6 characters long!");

        var r = hex.Substring(0, 2);
        var g = hex.Substring(2, 2);
        var b = hex.Substring(4, 2);

        string a = "";
        if (hex.Length >= 8)
            a = hex.Substring(6, 2);
        else
            a = "FF";

        return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
                         (int.Parse(g, NumberStyles.HexNumber) / 255f),
                         (int.Parse(b, NumberStyles.HexNumber) / 255f),
                         (int.Parse(a, NumberStyles.HexNumber) / 255f));
    }

    public static string FindBlindTitle(string name)
    {
        return BlindToTitle[name];
    }

    public static string FindNextBlind(string name)
    {
        var list = BlindToHex.Keys.ToList();
        var ix = list.IndexOf(name);
        if (ix == list.Count() - 1)
            return list[0];
        return list[ix + 1];
    }

    public static string FindPrevBlind(string name)
    {
        var list = BlindToHex.Keys.ToList();
        var ix = list.IndexOf(name);
        if (ix == 0)
            return list[list.Count() - 1];
        return list[ix - 1];
    }
}
