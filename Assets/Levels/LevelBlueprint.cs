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

    public int StageAmount => NextLetters.Length + 1;

    public string GetHintWord(int letterAmount)
    {
        foreach (string word in PossibleAnswers)
        {
            if (word.Length == letterAmount)
            {
                return word;
            }
        }

        Debug.Log("no word of this length in bank");
        return null;
    }
}

