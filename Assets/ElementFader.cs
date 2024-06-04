using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup GameplayBG;
    [SerializeField] private CanvasGroup GameplayGroup;

    [SerializeField] private CanvasGroup MainMenuBG;
    [SerializeField] private CanvasGroup MainMenuGroup;
    [SerializeField] private float duration;

    [Button]
    public void FadeIn()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(MainMenuGroup.DOFade(0, duration));

        sequence.Append(MainMenuBG.DOFade(0, duration));
        sequence.Join(GameplayBG.DOFade(1, duration));
        sequence.AppendInterval(duration);

        sequence.Append(GameplayGroup.DOFade(1, duration));

        sequence.Play();
    }
}