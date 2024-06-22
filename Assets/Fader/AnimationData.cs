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
    [Title("Guess Containers")]
    public TweenBlueprint guessMistakeAnimBlueprint;
    [FoldoutGroup("Gameplay")]
    public float correctGuessAnimaDelayBetweenLetters = 0.15f;
    [FoldoutGroup("Gameplay")]
    public float postCorrectGuessAnimDelay = 0f;

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

}
