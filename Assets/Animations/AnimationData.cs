using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

//[CreateAssetMenu(menuName ="Animation Data")]
public class AnimationData : ScriptableObject
{
    [FoldoutGroup("Translucent Config")]
    public Vector2Int TransluscentBlurConfig;
    [FoldoutGroup("Translucent Config")]
    public Vector2Int TransluscentGlassConfig;

    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint S_Fade_In;
    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint S_Fade_Out;

    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint G_Fade_In;
    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint G_Fade_Out;

    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint W_Fade_In;
    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint W_Fade_Out;
    
    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint Gift_Fade_In;
    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint Gift_Text_Fade_In;
    [FoldoutGroup("Transition Sequences")]
    public SequenceChainBlueprint Gift_Fade_Out;

    [FoldoutGroup("Button")]
    public ActionSequenceBlueprintRef ButtonClickAnimation;

    [FoldoutGroup("Gameplay/Guess Letter Bounce")]
    public ActionSequenceBlueprintRef GuessLetterBounce;
    [FoldoutGroup("Gameplay/Guess Letter Bounce")]
    public float CorrectGuessAnimDelayBetweenLetters = 0.15f;
    [FoldoutGroup("Gameplay/Guess Letter Bounce")]
    public float PostCorrectGuessAnimDelay = 0f;

    [FoldoutGroup("Gameplay/Correct Answer")]
    public float answerAnimDuration = 0.5f;
    [FoldoutGroup("Gameplay/Correct Answer")]
    public Ease answerAnimEase = Ease.OutQuad;
    [FoldoutGroup("Gameplay/Correct Answer")]
    public float postAnswerAnimDelay = 0f;
    [FoldoutGroup("Gameplay/Correct Answer")]
    public Color pluralWrongAnswerColor;
    
    [FoldoutGroup("Gameplay")]
    public ActionSequenceBlueprintRef AnswerHistoryMistakeAnimation;

    [FoldoutGroup("Gameplay")]
    public ActionSequenceBlueprintRef HintGuessContainerAnimation;
    [FoldoutGroup("Gameplay")]
    public ActionSequenceBlueprintRef GuessMistakeAnimBlueprint;
    [FoldoutGroup("Gameplay")]
    public ActionSequenceBlueprintRef ProgressionFruitAnimation;
   
    [FoldoutGroup("Tutorial")]
    public float swipeHandSwipeDuration;
    [FoldoutGroup("Tutorial")]
    public Ease swipeHandSwipeEase;
    [FoldoutGroup("Tutorial")]
    public float swipeHandFadeDuration;
    [FoldoutGroup("Tutorial")]
    public float swipeHandEndDelayDuration;

    [FoldoutGroup("Winning")]
    [Title("Level Bar")]
    public float sliderFillDuration;
    [FoldoutGroup("Winning")]
    public Ease sliderFillEase;

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
