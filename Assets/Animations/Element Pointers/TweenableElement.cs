using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class TweenableElement : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    public RectTransform RectTransform { get { return GetRectTransform(); } }

    [SerializeField] private CanvasGroup canvasGroup;
    public CanvasGroup CanvasGroup { get { return GetCanvasGroup(); } }

    [SerializeField] private Canvas canvas;
    public Canvas Canvas { get { return GetCanvas(); } }

    public void PopulateVariables()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }
    public Canvas GetCanvas()
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        return canvas;
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
