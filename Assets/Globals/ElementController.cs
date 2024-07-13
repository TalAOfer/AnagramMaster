using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using Sirenix.OdinInspector;

public class ElementController : SerializedMonoBehaviour
{
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;
    [SerializeField] private TranslucentImageSource TranslucentSource;
    [SerializeField] private GameplayManager GameplayManager;

    [SerializeField, DictionaryDrawerSettings(KeyLabel = "Element Data", ValueLabel = "Tweenable Element")]
    private Dictionary<TweenableElementData, TweenableElement> tweenableElementDict = new Dictionary<TweenableElementData, TweenableElement>();
    [SerializeField] private ElementSuperStates ElementSuperState;
    #region Transition Funcs 


    public void StartOpeningSequence()
    {
        CreateSequence(AnimData.IntroSequence).Play();
    }

    public void FadeStartMenuToGameplay()
    {
        StartCoroutine(StartMenuToGameplayRoutine());
    }

    public IEnumerator StartMenuToGameplayRoutine()
    {
        Sequence sequence = CreateSequence(AnimData.StartMenuToGameplayBlueprint).Play();
        yield return sequence.WaitForCompletion();
        StartCoroutine(GameplayManager.OnFadeInFinished());
    }

    public void FadeWinningNewAreaToGameplay()
    {
        CreateSequence(AnimData.WinningToGameplayNewAreaSequence).Play();
    }

    public IEnumerator FadeOutGameplayToWinning()
    {
        Sequence sequence = CreateSequence(AnimData.GameplayToWinningBlueprintOut);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator FadeInGameplayToWinning()
    {
        Sequence sequence = CreateSequence(AnimData.GameplayToWinningBlueprintIn);

        yield return sequence.WaitForCompletion();
    }

    public void WinningToGameplay()
    {
        CreateSequence(AnimData.WinningToGameplayNormalSequence).Play();
    }

    public void WinningToStartMenu()
    {
        CreateSequence(AnimData.WinningToStartMenuSequence).Play();
    }

    public IEnumerator PlayRegularEndOfWinningSequence()
    {
        Sequence sequence = CreateSequence(AnimData.WinningRegularEndingSequence);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator PlayNewAreaWinningSequence()
    {
        Sequence sequence = CreateSequence(AnimData.WinningNewAreaSequence);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator PlayNewBiomeWinningSequence()
    {
        Sequence sequence = CreateSequence(AnimData.WinningNewBiomeSequence);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator PlayFinishedGameWinningSequence()
    {
        Sequence sequence = CreateSequence(AnimData.WinningFinishedGameSequence);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator FadeGiftIn()
    {
        Sequence sequence = CreateSequence(AnimData.FadeGiftIn);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator FadeGiftOut()
    {
        Sequence sequence = CreateSequence(AnimData.FadeGiftOut);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator FadeGiftTextIn()
    {
        Sequence sequence = CreateSequence(AnimData.FadeInGiftText);

        yield return sequence.WaitForCompletion();
    }


    #endregion

    #region Helpers
    [Button]
    public void ChangeElementSuperState()
    {
        ElementVisibilityChart correctElementChart;

        switch (ElementSuperState)
        {
            case ElementSuperStates.GameStart:
                correctElementChart = ElementVisibilityChart.GameStart;
                break;
            case ElementSuperStates.StartMenu:
                correctElementChart = ElementVisibilityChart.StartMenu;
                break;
            case ElementSuperStates.Gameplay:
                correctElementChart = ElementVisibilityChart.Gameplay;
                break;
            case ElementSuperStates.Winning:
                correctElementChart = ElementVisibilityChart.Winning;
                break;
            case ElementSuperStates.Gift:
                correctElementChart = ElementVisibilityChart.Gift;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(correctElementChart), ElementSuperState, null);
        }

        foreach (var entry in tweenableElementDict)
        {
            TweenableElementData data = entry.Key;
            TweenableElement element = entry.Value;


            bool shouldEnable = data.ElementVisibilityChart.HasFlag(correctElementChart);
            ToggleElement(element, shouldEnable);
        }
        
    }

    public TweenableElement GetElement(TweenableElementData elementSO)
    {
        if (elementSO == null) 
        {
            Debug.Log("Element wasn't assigned in the animation sequence asset");
            return null;
        }

        if (tweenableElementDict.TryGetValue(elementSO, out var element) && element != null)
        {
            return element;
        }
        else
        {
            Debug.LogWarning($"Tweenable element for '{elementSO.name}' not found.");
            return null;
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

    #endregion

    #region Snap Funcs

    [Button]
    public void PopulateAllElementInnerVariables()
    {
        TweenableElement[] elements = FindObjectsOfType<TweenableElement>(true);
        foreach (TweenableElement element in elements)
        {
            element.PopulateVariables();
        }
    }

    
    public void ToggleElement(TweenableElement element, bool toggle)
    {
        int alpha = toggle ? 1 : 0;
        element.CanvasGroup.alpha = alpha;
        element.gameObject.SetActive(toggle);
    }

    public void ToggleInteractability(TweenableElement element, bool toggle)
    {
        element.CanvasGroup.interactable = toggle;
        element.CanvasGroup.blocksRaycasts = toggle;
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

    #endregion

    #region Over Time Funcs
    public Sequence FadeGroup(FaderTween blueprint)
    {
        TweenableElement element = GetElement(blueprint.Element);

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(blueprint.PreDelay);

        if (blueprint.SoundName != "")
        {
            sequence.AppendCallback(() => SoundManager.PlaySound(blueprint.SoundName, Vector3.zero));
        }

        if (blueprint.Fade is Fade.In)
        {
            if (blueprint.TransluscentSwitch != TransluscentSwitch.None)
            {
                sequence.AppendCallback(() => SwitchTransluscentType(blueprint.TransluscentSwitch));
            }

            sequence.AppendCallback(() => element.CanvasGroup.alpha = 0);
            sequence.AppendCallback(() => element.gameObject.SetActive(true));
        }

        sequence.Append(element.CanvasGroup.DOFade(blueprint.Fade is Fade.In ? 1 : 0, blueprint.Duration).SetEase(blueprint.Ease));

        foreach (ElementAnimation elementAnimation in blueprint.Animations.Value)
        {
            Sequence animationSequence = elementAnimation.GetAnimationSequence(element.RectTransform);

            if (elementAnimation.SequencingType is SequencingType.Append)
            {
                sequence.Append(animationSequence);
            }

            else
            {
                sequence.Join(animationSequence);
            }
        }

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




    #endregion

    private enum ElementSuperStates
    {
        GameStart = 1,
        StartMenu = 2,
        Gameplay = 4,
        Winning = 8,
        Gift = 16,
    }
}
