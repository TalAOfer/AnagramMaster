using DG.Tweening;
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
    public SequencingType SequencingType;
    public TransluscentSwitch TransluscentSwitch;
    public Fade Fade;
    public float Duration = 1;
    public float PostDelay = 0;
    public Ease Ease;
    public ElementAnimationBlueprint Animations;

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

