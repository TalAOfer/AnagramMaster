using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementAnimationBlueprint
{
    public List<ElementAnimation> Value;

    public Sequence GetSequence(RectTransform element)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (ElementAnimation elementAnimation in Value)
        {
            Sequence animationSequence = elementAnimation.GetAnimationSequence(element);

            if (elementAnimation.SequencingType is SequencingType.Append)
            {
                sequence.Append(animationSequence);
            }

            else
            {
                sequence.Join(animationSequence);
            }
        }

        return sequence;
    }
}

[Serializable]
public class ElementAnimation
{
    [ColoredField("GetSequencingTypeColor"), HideLabel]
    public SequencingType SequencingType = SequencingType.Join;
    [HorizontalGroup("Animation Archetype", 75), HideLabel]
    public ElementAnimationType AnimationType;
    [HorizontalGroup("Animation Archetype", 200), ShowIf("@ShouldShowSubtype()"), HideLabel]
    public ElementAnimationSubType Subtype;
    [HorizontalGroup("Animation Archetype", 200), HideLabel, HideIf("@ShouldShowSubtype()")]
    public ElementAnimationDirection AnimationDirection;
    [HorizontalGroup("Animation Archetype", 100), HideLabel]
    public Ease Ease;

    [HorizontalGroup("Delay", LabelWidth = 100)]
    public float PreDelay = 0;
    [HorizontalGroup("Delay")]
    public float PostDelay = 0;
    [FoldoutGroup("Values")]
    public float Duration = 1;
    [FoldoutGroup("Values")]
    [HideIf("@ShouldHideAmount()")]
    public float Amount;
    [FoldoutGroup("Values"), ShowIf("@ShouldShowPosition()")]
    public Vector2 Position;
    [FoldoutGroup("Values"), ShowIf("@ShouldShowSubtype()")]
    public int Vibrato = 10;
    [FoldoutGroup("Values"), ShowIf("@ShowShakeVariables()")]
    public float Randomness = 25f;
    [FoldoutGroup("Values"), ShowIf("@ShowPunchVariables()")]
    public float Elasticity = 1f;
    [FoldoutGroup("Values"), ShowIf("@ShowPunchVariables()")]
    public Vector3 Punch = Vector3.up;


    public Sequence GetAnimationSequence(RectTransform element)
    {
        Sequence sequence = DOTween.Sequence();
        Tween tween = null;

        sequence.AppendInterval(PreDelay);

        switch (AnimationType)
        {
            case ElementAnimationType.Zoom:
                Vector3 targetScale;
                if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
                {
                    targetScale = element.localScale;
                    sequence.AppendCallback(() => element.localScale *= Amount);
                }

                else
                {
                    targetScale = element.localScale *= Amount;
                }

                tween = element.DOScale(targetScale, Duration).SetEase(Ease);
                break;

            case ElementAnimationType.Slide:
                Vector2 targetPosition;
                if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
                {
                    targetPosition = element.anchoredPosition;
                    sequence.AppendCallback(() => element.anchoredPosition += Position);
                }

                else
                {
                    targetPosition = element.anchoredPosition + Position;
                }

                tween = element.DOAnchorPos(targetPosition, Duration).SetEase(Ease);
                break;

            case ElementAnimationType.Spin:
                Vector3 targetRotation;

                if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
                {
                    targetRotation = element.localEulerAngles;
                    Vector3 startingRotation = targetRotation + new Vector3(0, 0, Amount);

                    sequence.AppendCallback(() => element.localEulerAngles = startingRotation);
                }
                else
                {
                    targetRotation = element.localEulerAngles + new Vector3(0, 0, Amount);
                }
                tween = element.DORotate(targetRotation, Duration).SetEase(Ease);
                break;
            case ElementAnimationType.Jump:
                if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
                {
                    targetPosition = element.anchoredPosition;
                    sequence.AppendCallback(() => element.anchoredPosition += Position);
                }

                else
                {
                    targetPosition = element.anchoredPosition + Position;
                }

                tween = element.DOJumpAnchorPos(targetPosition, Amount, 1, Duration).SetEase(Ease);
                break;
            case ElementAnimationType.Shake:
                switch (Subtype)
                {
                    case ElementAnimationSubType.Rotation:
                        Debug.Log("Shake rotation shouldn't be used");
                        break;
                    case ElementAnimationSubType.Scale:
                        tween = element.DOShakeScale(Duration, Amount, Vibrato, Randomness).SetEase(Ease);
                        break;
                    case ElementAnimationSubType.Position:
                        tween = element.DOShakePosition(Duration, Amount, Vibrato, Randomness).SetEase(Ease);
                        break;
                }
                break;

            case ElementAnimationType.Punch:
                {

                    switch (Subtype)
                    {
                        case ElementAnimationSubType.Rotation:
                            tween = element.DOPunchRotation(Punch, Duration, Vibrato, Elasticity).SetEase(Ease);
                            break;
                        case ElementAnimationSubType.Scale:
                            tween = element.DOPunchScale(Punch, Duration, Vibrato, Elasticity).SetEase(Ease);
                            break;
                        case ElementAnimationSubType.Position:
                            tween = element.DOPunchAnchorPos(Punch, Duration, Vibrato, Elasticity).SetEase(Ease);
                            break;
                    }
                    break;
                }

        }


        if (tween != null)
        {
            sequence.Append(tween);
        }

        sequence.AppendInterval(PostDelay);

        return sequence;
    }

    private bool ShouldShowSubtype() => AnimationType is ElementAnimationType.Shake or ElementAnimationType.Punch;
    private bool ShowRotationVariables() => ShouldShowSubtype() && Subtype is ElementAnimationSubType.Rotation;
    private bool ShowPunchVariables() => AnimationType is ElementAnimationType.Punch;
    private bool ShowShakeVariables() => AnimationType is ElementAnimationType.Shake;
    private bool ShouldShowPosition() => AnimationType is ElementAnimationType.Jump or ElementAnimationType.Slide;
    private bool ShouldHideAmount() => AnimationType is ElementAnimationType.Punch;
    private Color GetSequencingTypeColor() => SequencingType == SequencingType.Append ? Color.red : Color.green;

    public enum ElementAnimationType
    {
        Zoom,
        Slide,
        Spin,
        Jump,
        Shake,
        Punch
    }

    public enum ElementAnimationSubType
    {
        Rotation,
        Scale,
        Position
    }

    public enum ElementAnimationDirection
    {
        FromValueToOriginal,
        FromOriginalToValue
    }
}
