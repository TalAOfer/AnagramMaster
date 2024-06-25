using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeHand : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Image image;
    [SerializeField] private LetterBank letterBank;
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;
    
    [Button]
    public void PlaySequence()
    {
        Sequence sequence = DOTween.Sequence();
        Color color = Color.white;
        color.a = 0f;
        image.color = color;
        gameObject.SetActive(true);

        for (int i = 0; i < letterBank.ActiveContainers.Count; i++)
        {
            if (i == 0)
            {
                container.anchoredPosition = letterBank.ActiveContainers[i].Rect.anchoredPosition;
                sequence.Append(image.DOFade(1, AnimData.swipeHandFadeDuration));
            }

            else
            {
                sequence.Append(container.DOAnchorPos(letterBank.ActiveContainers[i].Rect.anchoredPosition, AnimData.swipeHandSwipeDuration).SetEase(AnimData.swipeHandSwipeEase));
            }

            if (i == letterBank.ActiveContainers.Count - 1)
            {
                sequence.AppendInterval(AnimData.swipeHandEndDelayDuration);
                sequence.Append(image.DOFade(0, AnimData.swipeHandFadeDuration));
                sequence.AppendCallback(()=> gameObject.SetActive(false));
            }
        }

        sequence.Play();
    }
}
