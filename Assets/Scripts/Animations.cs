using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Animations
{
    public static Transform AnimatePosition(this Transform t, Vector3 to, float duration)
    {
        Animate(t, "pos", duration, t.position, to, Vector3.Lerp, p => t.position = p);
        return t;
    }

    public static Transform AnimateScale(this Transform t, Vector3 to, float duration)
    {
        Animate(t, "scale", duration, t.localScale, to, Vector3.Lerp, p => t.localScale = p);
        return t;
    }
    
    public static SpriteRenderer AnimateColor(this SpriteRenderer sr, Color to, float duration)
    {
        Animate(sr, "col", duration, sr.color, to, Color.Lerp, c => sr.color = c);
        return sr;
    }

    public static Image AnimateColor(this Image i, Color to, float duration)
    {
        Animate(i, "col", duration, i.color, to, Color.Lerp, c => i.color = c);
        return i;
    }

    public static CanvasGroup AnimateAlpha(this CanvasGroup c, float to, float duration)
    {
        Animate(c, "alpha", duration, c.alpha, to, Mathf.Lerp, a => c.alpha = a);
        return c;
    }

    class AnimationCoroutineContainer : MonoBehaviour
    {
        Dictionary<string, Coroutine> _idToRoutine = new Dictionary<string, Coroutine>();

        public void StartAnimation(IEnumerator anim, string id)
        {
            if (_idToRoutine.ContainsKey(id)) StopCoroutine(_idToRoutine[id]);
            _idToRoutine[id] = StartCoroutine(anim);
        }
    }

    static void Animate<T>(Component caller, string id, float duration, T start, T end, Func<T, T, float, T> lerper, Action<T> setter, Action endEvt = null)
    {
        var container = caller.GetComponent<AnimationCoroutineContainer>();
        if (container == null) container = caller.gameObject.AddComponent<AnimationCoroutineContainer>();
        container.StartAnimation(_Animate(duration, start, end, lerper, setter, endEvt), id);
    }

    static IEnumerator _Animate<T>(float duration, T start, T end, Func<T, T, float, T> lerper, Action<T> setter, Action endEvt)
    {
        float t = 0;
        setter(start);
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            setter(lerper(start, end, t));
            yield return null;
        }
        setter(end);
        endEvt?.Invoke();
    }
}