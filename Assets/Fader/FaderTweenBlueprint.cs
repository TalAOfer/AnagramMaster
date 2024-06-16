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
    public GameVisualElement Element;
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
    public ElementAnimationBlueprint Animations;

    private Color GetSequencingTypeColor()
    {

        return SequencingType == SequencingType.Append ? Color.red : Color.green;
    }
}

public enum GameVisualElement
{
    TranslucentOverlay,
    
    S_Logo,
    S_Button,
    S_BG,

    G_TopPanel,
    G_TopPanelElements,
    G_AnswerArea,
    G_LetterBank,
    G_BG,

    W_MainPanel,
    W_Banner,
    W_BannerText,
    W_Crown,
    W_FlagLeft,
    W_FlagRight,
    W_Flare,
    W_Particles,
    W_LevelText,
    W_LevelBar,
    W_BiomeText,
    W_BiomeBar,
    W_Button,
    G_BG_Secondary,
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

