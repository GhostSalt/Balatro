using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public List<UISelectable> Buttons;

    void Start()
    {
        foreach (var button in Buttons)
            button.OnInteract += delegate { Deactivate(); };
    }

    public void Activate()
    {
        foreach (var button in Buttons)
            button.Activate();
    }

    public void Deactivate()
    {
        foreach (var button in Buttons)
            button.Deactivate();
    }
}
