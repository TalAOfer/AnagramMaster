
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Sirenix.Utilities;

[Serializable]
public class ActionBlueprint
{
    [ColoredField("GetSequencingTypeColor"), HideLabel]
    public SequencingType SequencingType = SequencingType.Join;
    [HorizontalGroup("Animation Archetype", 100), HideLabel]
    public ElementAction ActionType;
    [HorizontalGroup("Delay", Width = 110), LabelWidth(65)]
    public float PreDelay = 0;
    [HorizontalGroup("Delay", Width = 110, MarginLeft = 20), LabelWidth(65)]
    public float PostDelay = 0;

    [ShowIf("ActionType", ElementAction.Zoom)]
    public ZoomActionData zoomData;
    [ShowIf("ActionType", ElementAction.Slide)]
    public SlideActionData slideData;
    [ShowIf("ActionType", ElementAction.Jump)]
    public JumpActionData jumpData;
    [ShowIf("ActionType", ElementAction.Spin)]
    public SpinActionData spinData;
    [ShowIf("ActionType", ElementAction.Shake)]
    public ShakeActionData shakeData;
    [ShowIf("ActionType", ElementAction.Punch)]
    public PunchActionData punchData;
    [ShowIf("ActionType", ElementAction.CallEvent)]
    public CallEventActionData callEventData;
    [ShowIf("ActionType", ElementAction.Fade)]
    public FadeActionData fadeData;
    [ShowIf("ActionType", ElementAction.PlaySound)]
    public PlaySoundActionData playSoundData;

    private bool ShouldShowPositionData() => ActionType is ElementAction.Slide or ElementAction.Jump;
    private bool ShouldHideSequencingType() => ActionType is ElementAction.CallEvent or ElementAction.PlaySound;

    [HideInInspector]
    public string GetDescription()
    {
        switch (ActionType)
        {
            case ElementAction.Zoom:
                return zoomData.GetDescription();

            case ElementAction.Slide:
                return slideData.GetDescription();

            case ElementAction.Spin:
                return spinData.GetDescription();

            case ElementAction.Jump:
                return jumpData.GetDescription();

            case ElementAction.Shake:
                return shakeData.GetDescription();

            case ElementAction.Punch:
                return punchData.GetDescription();

            case ElementAction.Fade:
                return fadeData.GetDescription();

            case ElementAction.CallEvent:
                return callEventData.GetDescription();

            case ElementAction.PlaySound:
                return playSoundData.GetDescription();
            default:
                return "";
        }
    }

    public Sequence GetSequence(TweenableElement element)
    {
        Sequence sequence = ActionType switch
        {
            ElementAction.Zoom => zoomData.GetActionSequence(element),
            ElementAction.Slide => slideData.GetActionSequence(element),
            ElementAction.Spin => spinData.GetActionSequence(element),
            ElementAction.Jump => jumpData.GetActionSequence(element),
            ElementAction.Shake => shakeData.GetActionSequence(element),
            ElementAction.Punch => punchData.GetActionSequence(element),
            ElementAction.Fade => fadeData.GetActionSequence(element),
            ElementAction.CallEvent => callEventData.GetActionSequence(element),
            ElementAction.PlaySound => playSoundData.GetActionSequence(element),
            _ => null,
        };

        sequence.PrependInterval(PreDelay);
        sequence.AppendInterval(PostDelay);

        return sequence;
    }

    private Color GetSequencingTypeColor() => SequencingType == SequencingType.Append ? Color.red : Color.green;
}

public enum ElementAction
{
    Zoom,
    Slide,
    Spin,
    Jump,
    Shake,
    Punch,
    Fade,
    CallEvent,
    PlaySound
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

[Serializable]
public abstract class ActionData
{
    [ReadOnly, ShowInInspector, HideLabel, MultiLineProperty(2)]
    private string description => GetDescription();

    public abstract Sequence GetActionSequence(TweenableElement element);
    public abstract string GetDescription();
}

[Serializable]
public class ZoomActionData : ActionData
{
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public ElementAnimationDirection AnimationDirection;
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public Ease Ease;
    public float Duration = 1;
    [Title("Value")]
    public Vector2 ScaleAddition;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        RectTransform elementTransform = element.RectTransform;
        Sequence sequence = DOTween.Sequence();

        Vector3 targetScale;
        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            targetScale = elementTransform.localScale;
            sequence.AppendCallback(() => elementTransform.localScale += (Vector3)ScaleAddition);
        }

        else
        {
            targetScale = elementTransform.localScale += (Vector3)ScaleAddition;
        }

        sequence.Append(elementTransform.DOScale(targetScale, Duration).SetEase(Ease));

        return sequence;
    }
    public override string GetDescription()
    {
        string desc = "";
        desc += "Scale element from ";

        if (AnimationDirection is ElementAnimationDirection.FromOriginalToValue)
        {
            desc += "original scale to ";

            if (ScaleAddition != Vector2.zero)
            {
                desc += "original scale + " + ScaleAddition.ToString("G4");
            }
        }

        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            desc += "original scale";

            if (ScaleAddition != Vector2.zero)
            {
                desc += " + " + ScaleAddition.ToString("G4");
            }

            desc += " to original scale";
        }

        return desc;
    }
}

[Serializable]
public class SlideActionData : ActionData
{
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public ElementAnimationDirection AnimationDirection;
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public Ease Ease;
    public float Duration = 1;
    [HorizontalGroup("Screen", LabelWidth = 100), Title("Value")]
    public bool OutOfScreen;
    [HorizontalGroup("Screen"), ShowIf("OutOfScreen"), Title("")]
    public Direction Direction;
    public Vector2 AddedValue;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        RectTransform elementTransform = element.RectTransform;
        Sequence sequence = DOTween.Sequence();

        Vector2 targetPosition;

        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            targetPosition = elementTransform.anchoredPosition;

            Vector2 startingPosition = elementTransform.anchoredPosition;

            if (OutOfScreen)
            {
                startingPosition = Tools.GetPositionOutsideScreen(element, Direction);
            }
            startingPosition += AddedValue;

            sequence.AppendCallback(() => elementTransform.anchoredPosition = startingPosition);
        }

        else
        {
            targetPosition = elementTransform.anchoredPosition;

            if (OutOfScreen)
            {
                targetPosition = Tools.GetPositionOutsideScreen(element, Direction);
            }

            targetPosition += AddedValue;
        }

        sequence.Append(elementTransform.DOAnchorPos(targetPosition, Duration).SetEase(Ease));

        return sequence;
    }

    public override string GetDescription()
    {
        string desc = "";
        desc += "Slide element from ";

        if (AnimationDirection is ElementAnimationDirection.FromOriginalToValue)
        {
            desc += "original position to ";

            if (OutOfScreen)
            {
                desc += Direction.ToString() + " of screen";
            }

            else
            {
                desc += "original position";
            }

            if (AddedValue != Vector2.zero)
            {
                desc += " + " + AddedValue.ToString("G4");
            }
        }

        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            if (OutOfScreen)
            {
                desc += Direction.ToString() + " of screen";
            }

            else
            {
                desc += "original position";
            }

            if (AddedValue != Vector2.zero)
            {
                desc += " + " + AddedValue.ToString("G4");
            }

            desc += " to original position";
        }

        return desc;
    }
}

[Serializable]
public class JumpActionData : ActionData
{
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public ElementAnimationDirection AnimationDirection;
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public Ease Ease;
    public float Duration = 1;
    [HorizontalGroup("Screen", LabelWidth = 100), Title("Value")]
    public bool OutOfScreen;
    [HorizontalGroup("Screen"), ShowIf("OutOfScreen"), Title("")]
    public Direction Direction;
    public float Power;
    public Vector2 AddedValue;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        RectTransform elementTransform = element.RectTransform;
        Sequence sequence = DOTween.Sequence();
        Vector2 jumpTargetPosition;

        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            jumpTargetPosition = elementTransform.anchoredPosition;

            Vector2 startingPosition = elementTransform.anchoredPosition;

            if (OutOfScreen)
            {
                startingPosition = Tools.GetPositionOutsideScreen(element, Direction);
            }
            startingPosition += AddedValue;

            sequence.AppendCallback(() => elementTransform.anchoredPosition = startingPosition);
        }

        else
        {
            jumpTargetPosition = elementTransform.anchoredPosition;

            if (OutOfScreen)
            {
                jumpTargetPosition = Tools.GetPositionOutsideScreen(element, Direction);
            }

            jumpTargetPosition += AddedValue;
        }

        sequence.Append(elementTransform.DOJumpAnchorPos(jumpTargetPosition, Power, 1, Duration).SetEase(Ease));
        return sequence;
    }
    public override string GetDescription()
    {
        string desc = "";
        desc += "Make element jump from ";

        if (AnimationDirection is ElementAnimationDirection.FromOriginalToValue)
        {
            desc += "original position to ";

            if (OutOfScreen)
            {
                desc += Direction.ToString() + " of screen";
            }

            else
            {
                desc += "original position";
            }

            if (AddedValue != Vector2.zero)
            {
                desc += " + " + AddedValue.ToString("G4");
            }
        }

        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            if (OutOfScreen)
            {
                desc += Direction.ToString() + " of screen";
            }

            else
            {
                desc += "original position";
            }

            if (AddedValue != Vector2.zero)
            {
                desc += " + " + AddedValue.ToString("G4");
            }

            desc += " to original position";
        }

        return desc;
    }

}

[Serializable]
public class SpinActionData : ActionData
{
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public ElementAnimationDirection AnimationDirection;
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public Ease Ease;
    public float Duration = 1;
    [Title("Value")]
    public float AddedValue;
    public override Sequence GetActionSequence(TweenableElement element)
    {
        RectTransform elementTransform = element.RectTransform;
        Sequence sequence = DOTween.Sequence();
        Vector3 targetRotation;

        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            targetRotation = elementTransform.localEulerAngles;
            Vector3 startingRotation = targetRotation + new Vector3(0, 0, AddedValue);

            sequence.AppendCallback(() => elementTransform.localEulerAngles = startingRotation);
        }
        else
        {
            targetRotation = elementTransform.localEulerAngles + new Vector3(0, 0, AddedValue);
        }

        sequence.Append(elementTransform.DORotate(targetRotation, Duration).SetEase(Ease));

        return sequence;
    }

    public override string GetDescription()
    {
        string desc = "";
        desc += "Spin element from ";

        if (AnimationDirection is ElementAnimationDirection.FromOriginalToValue)
        {
            desc += "original rotation to ";

            if (AddedValue != 0)
            {
                desc += "original rotation + " + AddedValue.ToString("G4");
            }
        }

        if (AnimationDirection is ElementAnimationDirection.FromValueToOriginal)
        {
            if (AddedValue != 0)
            {
                desc += "original rotation + " + AddedValue.ToString("G4");
            }

            desc += " to original rotation";
        }

        return desc;
    }

}

[Serializable]
public class ShakeActionData : ActionData
{
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public ElementAnimationSubType Subtype;
    [HorizontalGroup("Animation Archetype", 100), HideLabel]
    public Ease Ease;

    public float Duration = 1;
    [Title("Values")]
    [HideIf("Subtype", ElementAnimationSubType.Rotation)]
    public Vector2 Strength;
    [ShowIf("Subtype", ElementAnimationSubType.Rotation)]
    public float SpinStrength;
    public int Vibrato = 10;
    public float Randomness = 25f;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        RectTransform elementTransform = element.RectTransform;
        Sequence sequence = DOTween.Sequence();
        Tween tween = null;

        switch (Subtype)
        {
            case ElementAnimationSubType.Rotation:
                tween = elementTransform.DOShakeRotation(Duration, Vector3.forward * SpinStrength, Vibrato, Randomness).SetEase(Ease);
                break;
            case ElementAnimationSubType.Scale:
                tween = elementTransform.DOShakeScale(Duration, Strength, Vibrato, Randomness).SetEase(Ease);
                break;
            case ElementAnimationSubType.Position:
                tween = elementTransform.DOShakePosition(Duration, Strength, Vibrato, Randomness).SetEase(Ease);
                break;
        }

        sequence.Append(tween);

        return sequence;
    }
    public override string GetDescription()
    {
        return "Shake element's " + Subtype.ToString();
    }
}
[Serializable]
public class PunchActionData : ActionData
{
    [HorizontalGroup("Animation Archetype", 175), HideLabel]
    public ElementAnimationSubType Subtype;
    [HorizontalGroup("Animation Archetype", 100), HideLabel]
    public Ease Ease;
    public float Duration = 1;
    [Title("Values")]
    public Vector2 Punch;
    public int Vibrato = 10;
    public float Elasticity = 1;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        RectTransform elementTransform = element.RectTransform;
        Sequence sequence = DOTween.Sequence();
        Tween tween = null;

        switch (Subtype)
        {
            case ElementAnimationSubType.Rotation:
                tween = elementTransform.DOPunchRotation(Punch, Duration, Vibrato, Elasticity)
                    .SetEase(Ease);
                break;
            case ElementAnimationSubType.Scale:
                tween = elementTransform.DOPunchScale(Punch, Duration, Vibrato, Elasticity)
                    .SetEase(Ease);
                break;
            case ElementAnimationSubType.Position:
                tween = elementTransform.DOPunchAnchorPos(Punch, Duration, Vibrato, Elasticity)
                    .SetEase(Ease);
                break;
        }

        sequence.Append(tween);

        return sequence;
    }
    public override string GetDescription()
    {
        return "Punch element's " + Subtype.ToString();
    }
}

[Serializable]
public class FadeActionData : ActionData
{
    [HorizontalGroup("Animation Archetype", 85)]
    public bool SetActiveAccordingToFade = true;
    [HorizontalGroup("Animation Archetype", 45), HideLabel]
    public Fade Fade;
    [HorizontalGroup("Animation Archetype", 100), HideLabel]
    public Ease Ease;

    public float Duration = 1;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        CanvasGroup elementCanvasGroup = element.CanvasGroup;
        Sequence sequence = DOTween.Sequence();

        float fadeValue = Fade is Fade.In ? 1 : 0;
        sequence.Append(elementCanvasGroup.DOFade(fadeValue, Duration).SetEase(Ease));

        if (SetActiveAccordingToFade)
        {
            if (Fade is Fade.In)
            {
                sequence.PrependCallback(() => element.gameObject.SetActive(true));
            }

            else
            {
                sequence.AppendCallback(() => element.gameObject.SetActive(false));
            }
        }

        return sequence;
    }

    public override string GetDescription()
    {
        string desc;

        if (SetActiveAccordingToFade)
        {
            if (Fade is Fade.In)
            {
                desc = "Set element active and then fade element in";
            }

            else
            {
                desc = "Fade element out and then set element active";
            }
        }

        else
        {
            if (Fade is Fade.In)
            {
                desc = "Fade element in";
            }
            else
            {
                desc = "Fade element out";
            }
        }

        return desc;
    }
}

[Serializable]
public class CallEventActionData : ActionData
{
    public GameEvent GameEvent;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => GameEvent.Raise(element, element));
        return sequence;
    }

    public override string GetDescription()
    {
        string eventString = "-EnterGameEvent-";
        if (GameEvent != null)
        {
            eventString = GameEvent.ToString();
        }
        return "Raise " + eventString;
    }
}

[Serializable]
public class PlaySoundActionData : ActionData
{
    public string Sound;

    public override Sequence GetActionSequence(TweenableElement element)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => SoundManager.PlaySound(Sound, Vector3.zero));
        return sequence;
    }

    public override string GetDescription()
    {
        return "Play sound: '" + Sound + "'";
    }
}

public enum Fade
{
    In,
    Out
}

public enum SequencingType
{
    Append,
    Join
}