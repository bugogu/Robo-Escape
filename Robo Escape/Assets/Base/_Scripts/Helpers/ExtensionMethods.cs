using DG.Tweening;

public static class ExtensionMethods
{
    public static Tween FillImageAnimation(this UnityEngine.UI.Image fillImage, float startValue, float endValue, float duration)
    {
        return DOVirtual.Float(startValue, endValue, duration, x => fillImage.fillAmount = x);
    }
}
