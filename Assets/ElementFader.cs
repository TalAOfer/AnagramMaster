using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementFader : MonoBehaviour
{
    private AnimationData AnimData => AssetLocator.Instance.AnimationData;
    [SerializeField] private TranslucentImageSource TranslucentSource;

    [Title("Overlay")]
    [SerializeField] private CanvasGroup TranslucentOverlay;

    [Title("Gameplay")]
    [SerializeField] private CanvasGroup GameplayBG;
    //[SerializeField] private CanvasGroup GameplayTopPanel;
    [SerializeField] private CanvasGroup GameplayGuessContainers;
    [SerializeField] private CanvasGroup GameplayAnswer;
    [SerializeField] private CanvasGroup GameplayLetterBank;

    [Title("Start Menu")]
    [SerializeField] private CanvasGroup StartMenuBG;
    [SerializeField] private CanvasGroup StartMenuLogo;
    [SerializeField] private CanvasGroup StartMenuInteractables;

    [Title("Winning")]
    [SerializeField] private CanvasGroup WinningElements;


    public CanvasGroup GetElementCanvasGroup(GameVisualElement element)
    {
        switch (element)
        {
            case GameVisualElement.TransluscentOverlay:
                return TranslucentOverlay;
            case GameVisualElement.GameplayBG:
                return GameplayBG;
            //case GameVisualElement.GameplayTopPanel:
            //    return GameplayTopPanel;
            case GameVisualElement.GameplayGuessContainers:
                return GameplayGuessContainers;
            case GameVisualElement.GameplayAnswer:
                return GameplayAnswer;
            case GameVisualElement.GameplayLetterBank:
                return GameplayLetterBank;
            case GameVisualElement.WinningElements:
                return WinningElements;
            case GameVisualElement.StartMenuBG:
                return StartMenuBG;
            case GameVisualElement.StartMenuLogo:
                return StartMenuLogo;
            case GameVisualElement.StartMenuInteractables:
                return StartMenuInteractables;
            default:
                throw new ArgumentOutOfRangeException(nameof(element), element, null);
        }
    }

    public Sequence CreateSequence(FaderTweenBlueprint blueprint)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (FaderTween tween in blueprint.FadeSequence)
        {
            Sequence childSequence = FadeGroup(tween);
            
            if (tween.SequencingType is SequencingType.Append)
            {
                sequence.Append(childSequence);
            } 
            
            else
            {
                sequence.Join(childSequence);
            }
        }

        return sequence;
    }

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


    public Sequence FadeGroup(FaderTween blueprint)
    {
        CanvasGroup element = GetElementCanvasGroup(blueprint.Element);

        Sequence sequence = DOTween.Sequence();
        if (blueprint.Fade is Fade.In)
        {
            if (blueprint.TransluscentSwitch != TransluscentSwitch.None)
            {
                sequence.AppendCallback(() => SwitchTransluscentType(blueprint.TransluscentSwitch));
            }

            sequence.AppendCallback(() => element.alpha = 0);
            sequence.AppendCallback(() => element.gameObject.SetActive(true));
        }

        sequence.Append(element.DOFade(blueprint.Fade is Fade.In ? 1 : 0, blueprint.Duration).SetEase(blueprint.Ease));

        if (blueprint.Fade is Fade.Out)
        {
            sequence.AppendCallback(() => element.gameObject.SetActive(false));
            
            if (blueprint.TransluscentSwitch != TransluscentSwitch.None)
            {
                sequence.AppendCallback(() => SwitchTransluscentType(blueprint.TransluscentSwitch));
            }
        }

        if (blueprint.PostDelay > 0)
        {
            sequence.AppendInterval(blueprint.PostDelay);
        }

        return sequence;
    }

    private void Awake()
    {
        CreateSequence(AnimData.IntroSequence).Play();
    }

    public void FadeStartMenuToGameplay()
    {
        CreateSequence(AnimData.StartMenuToGameplayBlueprint).Play();
    }

    public IEnumerator FadeGameplayToWinning()
    {
        Sequence sequence = CreateSequence(AnimData.GameplayToWinningBlueprint);

        yield return sequence.WaitForCompletion();
    }

    public void WinningToGameplay()
    {
        CreateSequence(AnimData.WinningToGameplayBlueprint).Play();
    }

    public void ToggleInteractability(CanvasGroup canvasGroup, bool toggle)
    {
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;
    }

    private void SwitchTransluscentType(TransluscentSwitch transluscentSwitch)
    {
        var blurSettings = (ScalableBlurConfig)TranslucentSource.BlurConfig;

        switch (transluscentSwitch)
        {
            case TransluscentSwitch.None:
                break;
            case TransluscentSwitch.ToGlass:
                blurSettings.Iteration = AnimData.TransluscentGlassConfig.x;
                TranslucentSource.Downsample = AnimData.TransluscentGlassConfig.y;
                break;
            case TransluscentSwitch.ToBlur:
                blurSettings.Iteration = AnimData.TransluscentBlurConfig.x;
                TranslucentSource.Downsample = AnimData.TransluscentBlurConfig.y;
                break;
        }
    }


}
