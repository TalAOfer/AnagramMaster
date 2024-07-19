using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Animations/Tweenable Element", order = 1)]
public class TweenableElementPointer : ScriptableObject
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
    Gift = 16,
}

