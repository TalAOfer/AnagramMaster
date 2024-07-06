using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Tweenable Element")]
public class TweenableElementData : ScriptableObject
{
    public ElementVisibilityChart ElementVisibilityChart;
}

[Flags]
public enum ElementVisibilityChart
{
    GameStart = 1,
    StartMenu = 2,
    Gameplay = 4,
    Winning = 8,
}

