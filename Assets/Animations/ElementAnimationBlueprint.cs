using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementAnimationBlueprint
{
    public List<ElementAnimation> Value;
}

[Serializable]
public class ElementAnimation
{
    public ElementAnimationType AnimationType;
    public Ease Ease;
    public SequencingType sequencingType = SequencingType.Join;
    public float Duration = 1;
    public float Amount;
    [ShowIf("AnimationType", ElementAnimationType.SlideInto)]
    public Vector2 Direction;
    public float PostDelay = 0;
    public float PreDelay = 0;

    public Sequence GetAnimationSequence(RectTransform element)
    {
        Sequence sequence = DOTween.Sequence();
        Tween tween = null;

        sequence.AppendInterval(PreDelay);

        switch (AnimationType)
        {
            case ElementAnimationType.ZoomInto:
                Vector3 initialScale = element.localScale;
                sequence.AppendCallback(() => element.localScale *= Amount);
                tween = element.DOScale(initialScale, Duration).SetEase(Ease);
                break;

            case ElementAnimationType.SlideInto:
                Vector2 initialPosition = element.anchoredPosition;
                sequence.AppendCallback(() => element.anchoredPosition += Direction * Amount);
                tween = element.DOAnchorPos(initialPosition, Duration).SetEase(Ease);
                break;

            case ElementAnimationType.SpinInto:
                Vector3 initialRotation = element.localEulerAngles;
                Vector3 targetRotation = initialRotation + new Vector3(0, 0, Amount);

                sequence.AppendCallback(() => element.localEulerAngles = targetRotation);
                tween = element.DORotate(initialRotation, Duration).SetEase(Ease);
                break;
        }

        if (tween != null)
        {
            sequence.Append(tween);
        }

        sequence.AppendInterval(PostDelay);

        return sequence;
    }

    public enum ElementAnimationType
    {
        ZoomInto,
        SlideInto,
        SpinInto,
    }
}
