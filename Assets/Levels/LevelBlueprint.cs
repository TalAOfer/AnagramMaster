using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Levels/Level")]
public class LevelBlueprint : ScriptableObject
{
    public string StartingLetters;
    public string[] NextLetters;
    public string[] PossibleAnswers;
    public Color containerBG;

    public int StageAmount => NextLetters.Length + 1;
}

