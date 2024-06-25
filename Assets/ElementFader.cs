using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElementFader : MonoBehaviour
{
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;
    [SerializeField] private TranslucentImageSource TranslucentSource;
    [SerializeField] private GameplayManager GameplayManager;

    #region Elements

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement TranslucentOverlay;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement S_BG;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement S_Logo_Word;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement S_Logo_Journey;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement S_Logo_Backpacker;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement S_Button;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement G_BG;
    public Image CurrentActiveGameplayBackground { get { return G_BG.GetComponent<Image>(); } }

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement G_TopPanel;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement G_TopPanelElements;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement G_AnswerArea;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement G_LetterBank;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_MainPanel;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_Banner;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_BannerText;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_Crown;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_FlagLeft;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_FlagRight;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_Flare;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_Particles;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_LevelText;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_LevelBar;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_BiomeText;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_BiomeBar;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_Button;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_AreaImage;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement W_ExtraText;

    [FoldoutGroup("Elements")]
    [SerializeField] private TweenableElement G_BG_Secondary;
    public Image CurrentInactiveGameplayBackground { get { return G_BG_Secondary.GetComponent<Image>(); } }

    #endregion

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

    [Button]
    public void TestWinning()
    {
        Sequence sequence = CreateSequence(AnimData.GameplayToWinningBlueprintOut);
        sequence.Append(CreateSequence(AnimData.GameplayToWinningBlueprintIn));
        sequence.Play();
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


    #endregion

    #region Helpers

    public TweenableElement GetElement(GameVisualElement element)
    {
        switch (element)
        {
            case GameVisualElement.TranslucentOverlay:
                return TranslucentOverlay;
            case GameVisualElement.S_BG:
                return S_BG;
            case GameVisualElement.S_Logo_Word:
                return S_Logo_Word;
            case GameVisualElement.S_Button:
                return S_Button;
            case GameVisualElement.G_BG:
                return G_BG;
            case GameVisualElement.G_TopPanel:
                return G_TopPanel;
            case GameVisualElement.G_TopPanelElements:
                return G_TopPanelElements;
            case GameVisualElement.G_AnswerArea:
                return G_AnswerArea;
            case GameVisualElement.G_LetterBank:
                return G_LetterBank;
            case GameVisualElement.W_MainPanel:
                return W_MainPanel;
            case GameVisualElement.W_Banner:
                return W_Banner;
            case GameVisualElement.W_BannerText:
                return W_BannerText;
            case GameVisualElement.W_Crown:
                return W_Crown;
            case GameVisualElement.W_FlagLeft:
                return W_FlagLeft;
            case GameVisualElement.W_FlagRight:
                return W_FlagRight;
            case GameVisualElement.W_Flare:
                return W_Flare;
            case GameVisualElement.W_Particles:
                return W_Particles;
            case GameVisualElement.W_LevelText:
                return W_LevelText;
            case GameVisualElement.W_LevelBar:
                return W_LevelBar;
            case GameVisualElement.W_BiomeText:
                return W_BiomeText;
            case GameVisualElement.W_BiomeBar:
                return W_BiomeBar;
            case GameVisualElement.W_Button:
                return W_Button;
            case GameVisualElement.G_BG_Secondary:
                TweenableElement secondaryBG = G_BG_Secondary;
                (G_BG, G_BG_Secondary) = (G_BG_Secondary, G_BG);
                return secondaryBG;
            case GameVisualElement.W_ExtraText:
                return W_ExtraText;
            case GameVisualElement.W_AreaImage:
                return W_AreaImage;
            case GameVisualElement.S_Logo_Journey:
                return S_Logo_Journey;
            case GameVisualElement.S_Logo_Backpacker:
                return S_Logo_Backpacker;
            default:
                throw new ArgumentException("Invalid GameVisualElement");
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

    [Button]
    public void ResetToGameStart()
    {
        ToggleElement(S_BG, true);
        ToggleElement(S_Logo_Word, false);
        ToggleElement(S_Logo_Journey, false);
        ToggleElement(S_Logo_Backpacker, false);
        ToggleElement(S_Button, false);
        ToggleElement(TranslucentOverlay, false);

        ToggleElement(G_BG, false);
        ToggleElement(G_TopPanel, false);
        ToggleElement(G_TopPanelElements, false);
        ToggleElement(G_AnswerArea, false);
        ToggleElement(G_LetterBank, false);
        ToggleElement(W_MainPanel, false);
        ToggleElement(W_Banner, false);
        ToggleElement(W_BannerText, false);
        ToggleElement(W_Crown, false);
        ToggleElement(W_FlagLeft, false);
        ToggleElement(W_FlagRight, false);
        ToggleElement(W_Flare, false);
        ToggleElement(W_Particles, false);
        ToggleElement(W_LevelText, false);
        ToggleElement(W_LevelBar, false);
        ToggleElement(W_BiomeText, false);
        ToggleElement(W_BiomeBar, false);
        ToggleElement(W_Button, false);
        ToggleElement(W_AreaImage, false);
        ToggleElement(W_ExtraText, false);
    }

    [Button]
    public void ResetToStartMenu()
    {
        ToggleElement(S_BG, true);
        ToggleElement(S_Logo_Word, true);
        ToggleElement(S_Logo_Journey, true);
        ToggleElement(S_Logo_Backpacker, true);
        ToggleElement(S_Button, true);
        ToggleElement(TranslucentOverlay, true);

        ToggleElement(G_BG, false);
        ToggleElement(G_TopPanel, false);
        ToggleElement(G_TopPanelElements, false);
        ToggleElement(G_AnswerArea, false);
        ToggleElement(G_LetterBank, false);
        ToggleElement(W_MainPanel, false);
        ToggleElement(W_Banner, false);
        ToggleElement(W_BannerText, false);
        ToggleElement(W_Crown, false);
        ToggleElement(W_FlagLeft, false);
        ToggleElement(W_FlagRight, false);
        ToggleElement(W_Flare, false);
        ToggleElement(W_Particles, false);
        ToggleElement(W_LevelText, false);
        ToggleElement(W_LevelBar, false);
        ToggleElement(W_BiomeText, false);
        ToggleElement(W_BiomeBar, false);
        ToggleElement(W_Button, false);
        ToggleElement(W_AreaImage, false);
        ToggleElement(W_ExtraText, false);
    }

    [Button]
    public void ResetToGameplay()
    {
        ToggleElement(G_BG, true);
        ToggleElement(G_TopPanel, true);
        ToggleElement(G_TopPanelElements, true);
        ToggleElement(G_AnswerArea, true);
        ToggleElement(G_LetterBank, true);

        ToggleElement(TranslucentOverlay, false);
        ToggleElement(S_BG, false);
        ToggleElement(S_Logo_Word, false);
        ToggleElement(S_Logo_Journey, false);
        ToggleElement(S_Logo_Backpacker, false);
        ToggleElement(S_Button, false);
        ToggleElement(W_MainPanel, false);
        ToggleElement(W_Banner, false);
        ToggleElement(W_BannerText, false);
        ToggleElement(W_Crown, false);
        ToggleElement(W_FlagLeft, false);
        ToggleElement(W_FlagRight, false);
        ToggleElement(W_Flare, false);
        ToggleElement(W_Particles, false);
        ToggleElement(W_LevelText, false);
        ToggleElement(W_LevelBar, false);
        ToggleElement(W_BiomeText, false);
        ToggleElement(W_BiomeBar, false);
        ToggleElement(W_Button, false);
        ToggleElement(W_AreaImage, false);
        ToggleElement(W_ExtraText, false);
    }

    [Button]
    public void ResetToWinning()
    {
        ToggleElement(W_MainPanel, true);
        ToggleElement(W_Banner, true);
        ToggleElement(W_BannerText, true);
        ToggleElement(W_Crown, true);
        ToggleElement(W_FlagLeft, true);
        ToggleElement(W_FlagRight, true);
        ToggleElement(W_Flare, true);
        ToggleElement(W_Particles, true);
        ToggleElement(W_LevelText, true);
        ToggleElement(W_LevelBar, true);
        ToggleElement(W_BiomeText, true);
        ToggleElement(W_BiomeBar, true);
        ToggleElement(W_Button, true);

        ToggleElement(TranslucentOverlay, false);
        ToggleElement(S_BG, false);
        ToggleElement(S_Logo_Word, false);
        ToggleElement(S_Logo_Journey, false);
        ToggleElement(S_Logo_Backpacker, false);
        ToggleElement(S_Button, false);
        ToggleElement(G_BG, true);
        ToggleElement(G_TopPanel, false);
        ToggleElement(G_TopPanelElements, false);
        ToggleElement(G_AnswerArea, false);
        ToggleElement(G_LetterBank, false);
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

            if (elementAnimation.sequencingType is SequencingType.Append)
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

}
