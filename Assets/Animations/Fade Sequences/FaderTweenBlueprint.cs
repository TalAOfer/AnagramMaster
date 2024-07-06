using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fader Tween List")]
public class FaderTweenBlueprint : ScriptableObject
{
    public List<FaderTween> FadeSequence = new();
}

[Serializable]
public class FaderTween
{
    public TweenableElementData Element;
    [ColoredField("GetSequencingTypeColor")]
    public SequencingType SequencingType;
    public Fade Fade;
    [FoldoutGroup("")]
    public TransluscentSwitch TransluscentSwitch;
    [FoldoutGroup("")]
    public float Duration = 1;
    [FoldoutGroup("")]
    public float PostDelay = 0;
    [FoldoutGroup("")]
    public float PreDelay = 0f;
    [FoldoutGroup("")]
    public Ease Ease;
    [FoldoutGroup("")]
    public string SoundName = "";
    [FoldoutGroup("")]
    public ElementAnimationBlueprint Animations;

    private Color GetSequencingTypeColor()
    {

        return SequencingType == SequencingType.Append ? Color.red : Color.green;
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

public enum TransluscentSwitch
{
    None,
    ToGlass,
    ToBlur
}

