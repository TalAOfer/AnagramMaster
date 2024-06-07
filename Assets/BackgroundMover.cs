using DG.Tweening;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public RectTransform backgroundImage; // The RectTransform of your background image
    public float duration = 5f; // Duration of the animation

    void Start()
    {
        float screenWidth = Screen.width;
        float uiElementWidth = backgroundImage.rect.width * backgroundImage.localScale.x;
        float xAnchorPos = -(uiElementWidth - screenWidth);
        backgroundImage.DOAnchorPosX(xAnchorPos, duration).SetEase(Ease.Linear);
    }
}