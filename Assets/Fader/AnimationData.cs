using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName ="Animation Data")]
public class AnimationData : ScriptableObject
{
    [Title("Fade Sequences")]
    public FaderTweenBlueprint IntroSequence;
    public FaderTweenBlueprint StartMenuToGameplayBlueprint;
    public FaderTweenBlueprint GameplayToWinningBlueprint;
    public FaderTweenBlueprint WinningToGameplayBlueprint;

    [Title("Answer Addition")]
    public float answerAnimDuration = 0.5f;
    public Ease answerAnimEase = Ease.OutQuad;
    public float postAnswerAnimDelay = 0f;

    [Title("Guess Containers")]
    public TweenBlueprint guessMistakeAnimBlueprint;
    public float correctGuessAnimaDelayBetweenLetters = 0.15f;
    public float postCorrectGuessAnimDelay = 0f;

    [Title("Translucent Config")]
    public Vector2Int TransluscentBlurConfig;
    public Vector2Int TransluscentGlassConfig;
}
