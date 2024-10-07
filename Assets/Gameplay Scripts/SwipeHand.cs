using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipeHand : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Image image;
    [SerializeField] private LetterBank letterBank;
    [SerializeField] private TextMeshProUGUI swipeText;
    [SerializeField] private CanvasGroup HandGroup;
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;

    [Button]

    public void ActivateObject()
    {
        Color blank = GetBlank();
        image.color = blank;
        swipeText.color = blank;
        HandGroup.alpha = 1;
        HandGroup.gameObject.SetActive(true);
    }

    public Sequence PlayHandSequence()
    {
        Sequence sequence = DOTween.Sequence();

        image.color = GetBlank();

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
            }
        }

        sequence.Play();
        return sequence;
    }

    public Tween FadeInText()
    {
        swipeText.color = GetBlank();
        return swipeText.DOFade(1, 1f).Play();
    }

    public Sequence FadeOutEverything()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(HandGroup.DOFade(0, 1f));
        sequence.AppendCallback(() => HandGroup.gameObject.SetActive(false));
        return sequence.Play();
    }

    private Color GetBlank()
    {
        Color color = Color.white;
        color.a = 0f;
        return color;
    }
}
