using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
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
    public Fade Fade;
    public float Duration = 1;
    public float PostDelay = 0;
    public TransluscentSwitch TransluscentSwitch;
    public Ease Ease;
}


public enum GameVisualElement
{
    TransluscentOverlay,
    GameplayBG,
    GameplayTopPanel,
    GameplayGuessContainers,
    GameplayAnswer,
    GameplayLetterBank,
    WinningElements,
    StartMenuBG,
    StartMenuLogo,
    StartMenuInteractables
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

