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

    [FoldoutGroup("Start Menu")]
    public SequenceChainBlueprint S_Fade_In;
    [FoldoutGroup("Start Menu")]
    public SequenceChainBlueprint S_Fade_Out;

    [FoldoutGroup("Button")]
    public ActionSequenceBlueprintRef ButtonClickAnimation;

    [FoldoutGroup("Gameplay")]
    public SequenceChainBlueprint G_BG_Fade_In;
    [FoldoutGroup("Gameplay")]
    public SequenceChainBlueprint G_BG_Switch;
    [FoldoutGroup("Gameplay")]
    public SequenceChainBlueprint G_Fade_In;
    [FoldoutGroup("Gameplay")]
    public SequenceChainBlueprint G_Fade_Out;

    [FoldoutGroup("Gameplay")]
    public LoopingActionSequenceBlueprint CorrectGuessAnim;

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
    public ActionSequenceBlueprintRef ProgressionIconAnimation;
   
    [FoldoutGroup("Tutorial")]
    public float swipeHandSwipeDuration;
    [FoldoutGroup("Tutorial")]
    public Ease swipeHandSwipeEase;
    [FoldoutGroup("Tutorial")]
    public float swipeHandFadeDuration;
    [FoldoutGroup("Tutorial")]
    public float swipeHandEndDelayDuration;

    [FoldoutGroup("Winning")]
    public SequenceChainBlueprint W_Fade_In;
    [FoldoutGroup("Winning")]
    public SequenceChainBlueprint W_Fade_Out;
    [FoldoutGroup("Winning")]
    public SequenceChainBlueprint W_Button_Fade_In;
    [FoldoutGroup("Winning")]
    [Title("Bars")]
    public float sliderFillDuration;
    [FoldoutGroup("Winning")]
    public Ease sliderFillEase;

    [FoldoutGroup("Black Overlay")]
    public SequenceChainBlueprint Black_Overlay_In;
    [FoldoutGroup("Black Overlay")]
    public SequenceChainBlueprint Black_Overlay_Out;

    [FoldoutGroup("Gift")]
    public SequenceChainBlueprint Gift_Fade_In;
    [FoldoutGroup("Gift")]
    public SequenceChainBlueprint Gift_Text_Fade_In;
    [FoldoutGroup("Gift")]
    public SequenceChainBlueprint Gift_Fade_Out;
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
    [FoldoutGroup("Gift")]
    public string giftItemSoundName;

    [FoldoutGroup("Animals")]
    [Title("Solo")]
    public ActionSequenceBlueprint SoloAnimalFlashFade;
    [FoldoutGroup("Animals")]
    public Sprite YouFoundItSprite;
    [FoldoutGroup("Animals")]
    public Sprite FindTheAnimalSprite;
    [FoldoutGroup("Animals")]
    public SequenceChainBlueprint Animal_Reveal_Fade_In;
    [FoldoutGroup("Animals")]
    public SequenceChainBlueprint Animal_Reveal_Fade_Out;
    [FoldoutGroup("Animals")]
    public SequenceChainBlueprint Animal_Hidden_Fade_In;
    [FoldoutGroup("Animals")]
    public SequenceChainBlueprint Animal_Hidden_Fade_Out;



    [FoldoutGroup("Animals")]
    [Title("Album")]
    public SequenceChainBlueprint Animal_Album_In;
    [FoldoutGroup("Animals")]
    public SequenceChainBlueprint Animal_Album_Out;
    [FoldoutGroup("Animals")]
    public ActionSequenceBlueprint AnimalAlbumPhotoFadeIn;
    [FoldoutGroup("Animals")]
    public LoopingActionSequenceBlueprint FinishedAlbumAnim;
    [FoldoutGroup("Animals")]
    public ActionSequenceBlueprint Animal_Album_Text_In;
}
