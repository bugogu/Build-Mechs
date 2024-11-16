using DG.Tweening;

public static class ExtensionMethods
{
    public static Tween IncrementalText(this TMPro.TMP_Text tmpText, int startValue, int endValue, float duration)
    {
        return DOVirtual.Int(startValue, endValue, duration, x => tmpText.text = x.ToString());
    }

    public static Tween FillImageAnimation(this UnityEngine.UI.Image fillImage, float startValue, float endValue, float duration)
    {
        return DOVirtual.Float(startValue, endValue, duration, x => fillImage.fillAmount = x);
    }

    public static Tween AnimatedFloat(this float value, float startValue, float endValue, float duration)
    {
        return DOVirtual.Float(startValue, endValue, duration, x => value = x);
    }

    public static Tween SmoothPosition(this UnityEngine.Transform objectTransform, UnityEngine.Vector3 endPosition, float durationValue)
    {
        return objectTransform.DOMove(endPosition, durationValue);
    }
}
