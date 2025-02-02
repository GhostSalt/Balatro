using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectable : MonoBehaviour
{
    public KMSelectable Selectable;
    public Image Rend;
    public Sprite NormalSprite;
    public Sprite HighlightedSprite;
    public Sprite SelectedSprite;

    private float InitScale;
    private bool IsActive, IsHighlighted, IsSelected;

    private void Start()
    {
        Selectable.OnInteract += delegate { SelectablePress(); return false; };
        Selectable.OnInteractEnded += delegate { SelectableRelease(); };
        Selectable.OnHighlight += delegate { SelectableHL(); };
        Selectable.OnHighlightEnded += delegate { SelectableHLEnded(); };

        InitScale = Rend.transform.localScale.x;
        Rend.sprite = NormalSprite;
        Rend.transform.localScale = Selectable.transform.localScale = Vector3.zero;
    }

    protected virtual void SelectableHL()
    {
        IsHighlighted = true;
        UpdateState();
    }

    protected virtual void SelectableHLEnded()
    {
        IsHighlighted = false;
        UpdateState();
    }

    protected virtual void SelectablePress()
    {
        if (IsActive && !IsSelected)
        {
            IsSelected = true;
            UpdateState();
            RequestSound("press");
        }
    }

    protected virtual void SelectableRelease()
    {
        if (IsSelected)
        {
            IsSelected = false;
            UpdateState();
        }
    }

    protected virtual void UpdateState()
    {
        if (IsSelected)
            Rend.sprite = SelectedSprite;
        else if (IsHighlighted)
            Rend.sprite = HighlightedSprite;
        else
            Rend.sprite = NormalSprite;
    }

    public void Activate()
    {
        StartCoroutine(ActivateAnim());
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateAnim());
    }

    protected virtual IEnumerator ActivateAnim(float duration = 0.35f)
    {
        float timer = 0;
        while (timer < duration)
        {
            Rend.transform.localScale = Vector3.one * Easing.BackOut(timer, 0, InitScale, duration);
            yield return null;
            timer += Time.deltaTime;
        }

        Rend.transform.localScale = Vector3.one * InitScale;
        Selectable.transform.localScale = Vector3.one;
        IsActive = true;
    }

    protected virtual IEnumerator DeactivateAnim(float duration = 0.35f)
    {
        Selectable.transform.localScale = Vector3.zero;
        IsActive = false;

        float timer = 0;
        while (timer < duration)
        {
            Rend.transform.localScale = Vector3.one * Easing.InSine(timer, InitScale, 0, duration);
            yield return null;
            timer += Time.deltaTime;
        }

        Rend.transform.localScale = Vector3.zero;
    }

    public event Action<string, Transform> OnRequestSound;

    protected void RequestSound(string soundName)
    {
        OnRequestSound?.Invoke(soundName, transform);
    }
}
