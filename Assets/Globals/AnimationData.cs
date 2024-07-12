using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName ="Animation Data")]
public class AnimationData : ScriptableObject
{
    [FoldoutGroup("Translucent Config")]
    public Vector2Int TransluscentBlurConfig;
    [FoldoutGroup("Translucent Config")]
    public Vector2Int TransluscentGlassConfig;

    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint IntroSequence;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint StartMenuToGameplayBlueprint;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint GameplayToWinningBlueprintOut;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint GameplayToWinningBlueprintIn;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint WinningToGameplayNormalSequence;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint WinningToGameplayNewAreaSequence;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint WinningRegularEndingSequence;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint WinningNewAreaSequence;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint WinningNewBiomeSequence;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint WinningFinishedGameSequence;
    [FoldoutGroup("Transition Sequences")]
    public FaderTweenBlueprint WinningToStartMenuSequence;


    [FoldoutGroup("Gameplay")]
    [Title("Answer Addition")]
    public float answerAnimDuration = 0.5f;
    [FoldoutGroup("Gameplay")]
    public Ease answerAnimEase = Ease.OutQuad;
    [FoldoutGroup("Gameplay")]
    public float postAnswerAnimDelay = 0f;
    [FoldoutGroup("Gameplay")]
    public TweenBlueprint gameplayCollectibleAnimation;
    [FoldoutGroup("Gameplay")]
    public Color pluralWrongAnswerColor;
    [FoldoutGroup("Gameplay")]
    public TweenBlueprint answerHistoryMistakeAnimBlueprint;
    [FoldoutGroup("Gameplay")]
    [Title("Guess Containers")]
    public TweenBlueprint guessCorrectAnimBlueprint;
    [FoldoutGroup("Gameplay")]
    public TweenBlueprint guessHintAnimBlueprint;
    [FoldoutGroup("Gameplay")]
    public TweenBlueprint guessMistakeAnimBlueprint;
    [FoldoutGroup("Gameplay")]
    public float correctGuessAnimaDelayBetweenLetters = 0.15f;
    [FoldoutGroup("Gameplay")]
    public float postCorrectGuessAnimDelay = 0f;
    [FoldoutGroup("Gameplay")]
    public float swipeHandSwipeDuration;
    [FoldoutGroup("Gameplay")]
    public Ease swipeHandSwipeEase;
    [FoldoutGroup("Gameplay")]
    public float swipeHandFadeDuration;
    [FoldoutGroup("Gameplay")]
    public float swipeHandEndDelayDuration;

    [FoldoutGroup("Winning")]
    [Title("Level Bar")]
    public float sliderFillDuration;
    [FoldoutGroup("Winning")]
    public Ease sliderFillEase;

    [FoldoutGroup("Winning")]
    [Title("Biome Bar")]
    public TweenBlueprint BiomeCurrentCollectibleAmountAnimation;
    [FoldoutGroup("Winning")]
    public TweenBlueprint TotalCollectibleImageAnimation;
    [FoldoutGroup("Winning")]
    public float TotalCollectibleImageUpscaleAmount = 1.1f;
    [FoldoutGroup("Winning")]
    public float NewBiomeImageUpscale = 1.25f;



    [FoldoutGroup("Gift")]
    [Title("Parent")]
    public float giftParentDistanceFromCenter;
    [FoldoutGroup("Gift")]
    public float giftAppearanceDuration;
    [FoldoutGroup("Gift")]
    public Ease giftAppearanceEase;
    [Title ("Opening Animation")]
    [FoldoutGroup("Gift")]
    public ElementAnimationBlueprint GiftPreparationAnimation;
    [FoldoutGroup("Gift")]
    public ElementAnimationBlueprint GiftOpeningAnimation;
    [FoldoutGroup("Gift")]
    [Title("Items")]
    public float giftItemAppearanceDuration;
    [FoldoutGroup("Gift")]
    public Ease giftItemAppearanceEase;
    [FoldoutGroup("Gift")]
    public float defaultGiftItemHeight;
    [FoldoutGroup("Gift")]
    public float giftItemHeightSpacing;
    [FoldoutGroup("Gift")]
    public float giftItemDelaySpacing;

}
