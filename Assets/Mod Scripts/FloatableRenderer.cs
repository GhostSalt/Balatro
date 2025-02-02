using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatableRenderer : MonoBehaviour
{
    protected SpriteRenderer BaseRend;

    public virtual void Initialise(Sprite baseSprite, SpriteRenderer baseTemplate)
    {
        BaseRend = CreateRend(baseTemplate);
        BaseRend.sprite = baseSprite;
        BaseRend.transform.localPosition = Vector3.zero;
        StartCoroutine(Animate());

        SetScale(0);
    }

    protected SpriteRenderer CreateRend(SpriteRenderer template)
    {
        var rend = Instantiate(template, transform);
        rend.gameObject.SetActive(true);
        return rend;
    }

    protected void SetBaseSprite(Sprite sprite)
    {
        BaseRend.sprite = sprite;
    }

    public Vector3 GetLocation()
    {
        return transform.localPosition;
    }

    public void SetLocation(Vector3 location)
    {
        transform.localPosition = location;
    }

    public void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }

    public virtual void Appear(float scale = 1f)
    {
        StartCoroutine(AppearAnim(scale));
    }

    protected IEnumerator AppearAnim(float scale, float duration = 0.25f)
    {
        float timer = 0;
        while (timer < duration)
        {
            SetScale(Easing.BackOut(timer, 0, scale, duration));
            yield return null;
            timer += Time.deltaTime;
        }
        SetScale(scale);
    }

    public virtual void Disappear()
    {
        StartCoroutine(DisappearAnim(1));
    }

    protected IEnumerator DisappearAnim(float scale, float duration = 0.25f)
    {
        float timer = 0;
        while (timer < duration)
        {
            SetScale(Easing.InSine(timer, scale, 0, duration));
            yield return null;
            timer += Time.deltaTime;
        }
        SetScale(0);
    }

    public virtual void SetZ(int z)
    {
        SetBaseZ(z);
    }

    private void SetBaseZ(int z)
    {
        BaseRend.sortingOrder = z * 10;
    }

    private IEnumerator Animate(float angleBoundary = 8f, float speedX = Mathf.PI / 8f / 5f, float speedZ = 2.718281828459045235360287471352f / 5f / 5f)
    {
        while (true)
        {
            var x = Mathf.Lerp(-angleBoundary, angleBoundary, (Mathf.Sin(Mathf.PI * 2 * ((Time.time + transform.localPosition.x + transform.localPosition.y) * speedX % 1)) + 1) / 2);
            var z = Mathf.Lerp(-angleBoundary, angleBoundary, (Mathf.Sin(Mathf.PI * 2 * ((Time.time + transform.localPosition.x + transform.localPosition.y) * speedZ % 1)) + 1) / 2);
            transform.localRotation = Quaternion.Euler(z, x, 0);
            yield return null;
        }
    }

    public void Delete()
    {
        StartCoroutine(DestroyLogic());
    }

    protected virtual IEnumerator DestroyLogic()
    {
        yield return StartCoroutine(DisappearAnim(1));
        Destroy(BaseRend.gameObject);
        Destroy(gameObject);
    }
}
