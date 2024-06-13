using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup TranslucentOverlay;
    [SerializeField] private float elementDelay = 0.5f;
    [SerializeField] private float crossFadeDelay = 1;

    [Title("Gameplay")]
    [SerializeField] private CanvasGroup GameplayBG;
    [SerializeField] private CanvasGroup GameplayTopPanel;
    [SerializeField] private CanvasGroup GameplayGuessContainers;
    [SerializeField] private CanvasGroup GameplayAnswer;
    [SerializeField] private CanvasGroup GameplayLetterBank;

    [Title("Start Menu")]
    [SerializeField] private CanvasGroup StartMenuBG;
    [SerializeField] private CanvasGroup StartMenuLogo;
    [SerializeField] private CanvasGroup StartMenuInteractables;

    [Title("Winning")]
    [SerializeField] private CanvasGroup WinningElements;


    [Button]
    public void ResetToMenu(bool withElements)
    {
        ToggleGroup(StartMenuBG, true);
        ToggleGroup(StartMenuLogo, withElements);
        ToggleGroup(StartMenuInteractables, withElements);
        ToggleGroup(TranslucentOverlay, withElements);

        ToggleGroup(GameplayBG, false);
        ToggleGroup(GameplayGuessContainers, false);
        ToggleGroup(GameplayLetterBank, false);
        
        ToggleGroup(WinningElements, false);
    }

    [Button]
    public void ResetToGameplay()
    {
        ToggleGroup(GameplayBG, true);
        ToggleGroup(GameplayGuessContainers, true);
        ToggleGroup(GameplayLetterBank, true);

        ToggleGroup(StartMenuBG, false);
        ToggleGroup(StartMenuInteractables, false);
        ToggleGroup(TranslucentOverlay, false);

        ToggleGroup(WinningElements, false);
    }
    
    public void ToggleGroup(CanvasGroup group, bool toggle)
    {
        int alpha = toggle ? 1 : 0;
        group.alpha = alpha;
        group.gameObject.SetActive(toggle);
    }

    private void Awake()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(FadeGroup(TranslucentOverlay, Fade.In, FadeDuration.Element));

        sequence.AppendInterval(elementDelay);

        sequence.Append(FadeGroup(StartMenuLogo, Fade.In, FadeDuration.Element));
        
        sequence.AppendInterval(elementDelay);

        sequence.Append(FadeGroup(StartMenuInteractables, Fade.In, FadeDuration.Element));
    }

    public void FadeStartMenuToGameplay()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(FadeGroup(StartMenuLogo, Fade.Out, FadeDuration.Element));
        sequence.Append(FadeGroup(StartMenuInteractables, Fade.Out, FadeDuration.Element));
        sequence.Append(FadeGroup(StartMenuBG, Fade.Out, FadeDuration.Crossfade));
        sequence.Join(FadeGroup(TranslucentOverlay, Fade.Out, FadeDuration.Crossfade));
        sequence.Join(FadeGroup(GameplayBG, Fade.In, FadeDuration.Crossfade));

        //sequence.Append(FadeGroup(GameplayTopPanel, Fade.In, FadeDuration.Element));
        sequence.Append(FadeGroup(GameplayGuessContainers, Fade.In, FadeDuration.Element));
        sequence.Join(FadeGroup(GameplayAnswer, Fade.In, FadeDuration.Element));
        sequence.Append(FadeGroup(GameplayLetterBank, Fade.In, FadeDuration.Element));

        sequence.Play();
    }

    public IEnumerator GameplayFinish()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(FadeGroup(GameplayGuessContainers, Fade.Out, FadeDuration.Element));
        sequence.Join(FadeGroup(GameplayLetterBank, Fade.Out, FadeDuration.Element));
        //sequence.Join(FadeGroup(GameplayTopPanel, Fade.Out, FadeDuration.Element));
        sequence.Join(FadeGroup(GameplayGuessContainers, Fade.Out, FadeDuration.Element));
        sequence.Join(FadeGroup(GameplayAnswer, Fade.Out, FadeDuration.Element));

        sequence.AppendInterval(crossFadeDelay);
        sequence.Append(FadeGroup(WinningElements, Fade.In, FadeDuration.Element));

        yield return sequence.WaitForCompletion();
    }

    public void WinningToGameplay()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(FadeGroup(WinningElements, Fade.Out, FadeDuration.Element));
        sequence.AppendInterval(crossFadeDelay);
        //sequence.Append(FadeGroup(GameplayTopPanel, Fade.In, FadeDuration.Element));
        sequence.Append(FadeGroup(GameplayGuessContainers, Fade.In, FadeDuration.Element));
        sequence.Join(FadeGroup(GameplayAnswer, Fade.In, FadeDuration.Element));
        sequence.Append(FadeGroup(GameplayLetterBank, Fade.In, FadeDuration.Element));
    }

    public void ToggleInteractability(CanvasGroup canvasGroup, bool toggle)
    {
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;
    }

    public Sequence FadeGroup(CanvasGroup group, Fade fade, FadeDuration delay)
    {
        Sequence sequence = DOTween.Sequence();
        if (fade is Fade.In)
        {
            sequence.AppendCallback(() => group.alpha = 0);
            sequence.AppendCallback(() => group.gameObject.SetActive(true));
        }

        sequence.Append(group.DOFade(fade is Fade.In ? 1 : 0, GetFadeDuration(delay)));

        if (fade is Fade.Out)
        {
            sequence.AppendCallback(() => group.gameObject.SetActive(false));
        }

        return sequence;
    }

    private float GetFadeDuration(FadeDuration delay)
    {
        switch (delay)
        {
            case FadeDuration.Element:
                return elementDelay;
            case FadeDuration.Crossfade:
                return crossFadeDelay;
            default:
                Debug.Log("No delay type");
                return 0;

        }
    }
}

public enum Fade
{
    In,
    Out
}

public enum FadeDuration
{
    Element,
    Crossfade
}