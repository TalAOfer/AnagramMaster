using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup GameplayBG;
    [SerializeField] private CanvasGroup GameplayBaseGroup;
    [SerializeField] private CanvasGroup GameplayTransluscentGroup;

    [SerializeField] private CanvasGroup MainMenuBG;
    [SerializeField] private CanvasGroup MainMenuOverlay;
    [SerializeField] private CanvasGroup MainMenuElements;

    [SerializeField] private CanvasGroup WinningElements;

    [SerializeField] private float duration;

    [Button]
    public void MainMenuToGameplay()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(FadeMainMenuElements(false));

        sequence.AppendCallback(()=> ToggleInteractability(MainMenuElements, false));

        sequence.AppendInterval(duration);

        sequence.Append(FadeMainMenuBG(false));
        sequence.Join(FadeGameplayBG(true));

        sequence.AppendInterval(duration);

        sequence.Append(FadeGameplayElements(true));

        sequence.AppendCallback(() => ToggleInteractability(MainMenuBG, false));
        sequence.AppendCallback(() => ToggleInteractability(MainMenuOverlay, false));
        sequence.AppendCallback(() => ToggleInteractability(GameplayBaseGroup, true));
        sequence.AppendCallback(() => ToggleInteractability(GameplayTransluscentGroup, true));

        sequence.Play();
    }

    public IEnumerator GameplayFinish()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(FadeGameplayElements(false));
        sequence.AppendCallback(() => ToggleInteractability(GameplayBaseGroup, false));
        sequence.AppendCallback(() => ToggleInteractability(GameplayTransluscentGroup, false));
        sequence.AppendInterval(duration);
        sequence.Append(FadeCanvasGroup(WinningElements, true));

        yield return sequence.WaitForCompletion();
    }

    public void WinningToGameplay()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(FadeCanvasGroup(WinningElements, false));
        sequence.AppendCallback(() => ToggleInteractability(GameplayBaseGroup, true));
        sequence.AppendCallback(() => ToggleInteractability(GameplayTransluscentGroup, true));
        sequence.AppendInterval(duration);
        sequence.Append(FadeGameplayElements(true));
    }

    public void ToggleInteractability(CanvasGroup canvasGroup, bool toggle)
    {
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;
    }

    public Tween FadeCanvasGroup(CanvasGroup canvasGroup, bool fadeIn) => canvasGroup.DOFade(fadeIn ? 1 : 0, duration);

    public Sequence FadeGameplayElements(bool fadeIn)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(FadeCanvasGroup(GameplayBaseGroup, fadeIn));
        sequence.Join(FadeCanvasGroup(GameplayTransluscentGroup, fadeIn));
        return sequence;
    }

    public Tween FadeGameplayBG(bool fadeIn)
    {
        return FadeCanvasGroup(GameplayBG, fadeIn);
    }

    public Tween FadeMainMenuElements(bool fadeIn)
    {
        return FadeCanvasGroup(MainMenuElements, fadeIn);
    }

    public Sequence FadeMainMenuBG(bool fadeIn)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(FadeCanvasGroup(MainMenuBG, fadeIn));
        sequence.Join(FadeCanvasGroup(MainMenuOverlay, fadeIn));
        return sequence;
    }
}