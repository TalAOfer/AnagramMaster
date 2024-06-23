using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class TweenableElement : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    public RectTransform RectTransform { get { return GetRectTransform(); } }

    [SerializeField] private CanvasGroup canvasGroup;
    public CanvasGroup CanvasGroup { get { return GetCanvasGroup(); } }

    public void PopulateVariables()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public RectTransform GetRectTransform()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        return rectTransform;
    }

    public CanvasGroup GetCanvasGroup()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        return canvasGroup;
    }
}
