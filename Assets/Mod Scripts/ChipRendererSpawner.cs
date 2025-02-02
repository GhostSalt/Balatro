using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChipRendererSpawner : MonoBehaviour
{
    public Sprite[] AllChipSprites;
    public SpriteRenderer ChipTemplate;

    void Start()
    {
        ChipTemplate.gameObject.SetActive(false);
    }

    public ChipRenderer SpawnChip(string name, Vector3 location)
    {
        var obj = new GameObject(name);
        obj.transform.parent = ChipTemplate.transform.parent;
        var rend = obj.AddComponent<ChipRenderer>();
        rend.Initialise(new Blind(name), FindChipSprites(name), ChipTemplate);
        rend.SetLocation(location);

        return rend;
    }

    private Sprite[] FindChipSprites(string name)
    {
        var cands = new List<Sprite>();
        foreach (Sprite candidate in AllChipSprites)
            if (Regex.IsMatch(candidate.name, "^" + name + " [0-9]+$"))
                cands.Add(candidate);
        return cands.ToArray();
    }
}
