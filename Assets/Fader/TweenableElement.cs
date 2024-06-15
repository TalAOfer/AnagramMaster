using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class TweenableElement : MonoBehaviour
{
    public RectTransform RectTransform {  get; private set; }
    public CanvasGroup CanvasGroup { get; private set; }
    private void OnValidate()
    {
        if (RectTransform == null)
        {
            RectTransform = GetComponent<RectTransform>();
        }

        if (CanvasGroup == null)
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
