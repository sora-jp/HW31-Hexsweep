using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class Animations
{
    class AnimatorCoroutineContainer : MonoBehaviour { }

    public static SpriteRenderer AnimateColor(this SpriteRenderer renderer, Color toColor, float duration = 1f)
    {
        var container = renderer.gameObject.AddComponent<AnimatorCoroutineContainer>();
        container.StartCoroutine(_AnimateColor(renderer, toColor, duration, () => GameObject.Destroy(container)));
        return renderer;
    }

    public static Image AnimateColor(this Image image, Color toColor, float duration = 1f)
    {
        image.StartCoroutine(_AnimateColor(image, toColor, duration));
        return image;
    }

    public static Transform AnimateScale(this Transform trans, Vector3 toScale, float duration = 1f)
    {
        var container = trans.gameObject.GetComponent<AnimatorCoroutineContainer>();
        container?.StopAllCoroutines();
        if (container == null) container = trans.gameObject.AddComponent<AnimatorCoroutineContainer>();
        container.StartCoroutine(_AnimateScale(trans, toScale, duration, () => GameObject.Destroy(container)));
        return trans;
    }

    public static Transform AnimatePosition(this Transform trans, Vector3 toPos, float duration = 1f)
    {
        var container = trans.gameObject.AddComponent<AnimatorCoroutineContainer>();
        container.StartCoroutine(_AnimatePosition(trans, toPos, duration, () => GameObject.Destroy(container)));
        return trans;
    }

    public static CanvasGroup AnimateAlpha(this CanvasGroup group, float toAlpha, float duration = 1f)
    {
        var container = group.gameObject.AddComponent<AnimatorCoroutineContainer>();
        container.StartCoroutine(_AnimateAlpha(group, toAlpha, duration, () => GameObject.Destroy(container)));
        return group;
    }


    static IEnumerator _AnimatePosition(Transform trans, Vector3 toPos, float duration, Action end) => Animate(trans.position, toPos, duration, Vector3.Lerp, p => trans.position = p, end);
    static IEnumerator _AnimateAlpha(CanvasGroup group, float toAlpha, float duration, Action end) => Animate(group.alpha, toAlpha, duration, Mathf.Lerp, a => group.alpha = a, end);
    static IEnumerator _AnimateColor(SpriteRenderer renderer, Color toColor, float duration, Action end) => Animate(renderer.color, toColor, duration, Color.Lerp, c => renderer.color = c, end);
    static IEnumerator _AnimateColor(Image image, Color toColor, float duration) => Animate(image.color, toColor, duration, Color.Lerp, c => image.color = c);
    static IEnumerator _AnimateScale(Transform trans, Vector3 toScale, float duration, Action end) => Animate(trans.localScale, toScale, duration, Vector3.Lerp, s => trans.localScale = s, end);

    const float a = 3;

    private static IEnumerator Animate<T>(T fromVal, T toVal, float duration, Func<T, T, float, T> lerper, Action<T> setter, Action end = null)
    {
        float t = 0;
        setter(fromVal);
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            setter(lerper(fromVal, toVal, Mathf.Pow(t, a) / (Mathf.Pow(t, a) + Mathf.Pow(1-t, a))));
            yield return null;
        }
        setter(toVal);
        end?.Invoke();
    }
}
