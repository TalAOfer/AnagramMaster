using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class ElementFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup TranslucentOverlay;

    [SerializeField] private CanvasGroup GameplayBG;
    [SerializeField] private CanvasGroup GameplayElementsBase;
    [SerializeField] private CanvasGroup GameplayTranslucent;

    [SerializeField] private CanvasGroup StartMenuBG;
    [SerializeField] private CanvasGroup StartMenuElements;

    [SerializeField] private CanvasGroup WinningElements;

    [SerializeField] private float duration;

    [Button]
    public void ResetToMenu()
    {
        ToggleGroup(StartMenuBG, true);
        ToggleGroup(StartMenuElements, true);
        ToggleGroup(TranslucentOverlay, true);

        ToggleGroup(GameplayBG, false);
        ToggleGroup(GameplayElementsBase, false);
        ToggleGroup(GameplayTranslucent, false);
        
        ToggleGroup(WinningElements, false);
    }

    [Button]
    public void ResetToGameplay()
    {
        ToggleGroup(GameplayBG, true);
        ToggleGroup(GameplayElementsBase, true);
        ToggleGroup(GameplayTranslucent, true);

        ToggleGroup(StartMenuBG, false);
        ToggleGroup(StartMenuElements, false);
        ToggleGroup(TranslucentOverlay, false);

        ToggleGroup(WinningElements, false);
    }
    
    public void ToggleGroup(CanvasGroup group, bool toggle)
    {
        int alpha = toggle ? 1 : 0;
        group.alpha = alpha;
        group.gameObject.SetActive(toggle);
    } 

    public void FadeStartMenuToGameplay()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(FadeGroup(StartMenuElements, false));

        sequence.AppendInterval(duration);

        sequence.Append(FadeGroup(StartMenuBG, false));
        sequence.Join(FadeGroup(TranslucentOverlay, false));
        sequence.Join(FadeGroup(GameplayBG, true));

        sequence.AppendInterval(duration);

        sequence.Append(FadeGroup(GameplayElementsBase, true));
        sequence.Join(FadeGroup(GameplayTranslucent, true));

        sequence.Play();
    }

    public IEnumerator GameplayFinish()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(FadeGroup(GameplayElementsBase, false));
        sequence.Join(FadeGroup(GameplayTranslucent, false));
        sequence.AppendInterval(duration);
        sequence.Append(FadeGroup(WinningElements, true));

        yield return sequence.WaitForCompletion();
    }

    public void WinningToGameplay()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(FadeGroup(WinningElements, false));
        sequence.AppendInterval(duration);
        sequence.Append(FadeGroup(GameplayElementsBase, true));
        sequence.Join(FadeGroup(GameplayTranslucent, true));
    }

    public void ToggleInteractability(CanvasGroup canvasGroup, bool toggle)
    {
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;
    }

    public Sequence FadeGroup(CanvasGroup group, bool fadeIn)
    {
        Sequence sequence = DOTween.Sequence();
        if (fadeIn)
        {
            sequence.AppendCallback(() => group.alpha = 0);
            sequence.AppendCallback(() => group.gameObject.SetActive(true));
        }

        sequence.Append(group.DOFade(fadeIn ? 1 : 0, duration));

        if (!fadeIn)
        {
            sequence.AppendCallback(() => group.gameObject.SetActive(false));
        }

        return sequence;
    }

}