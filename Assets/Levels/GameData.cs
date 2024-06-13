using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public GameData() 
    {
        IsInitialized = false;
        Index = 0;
        CurrentLetters = "";
        NextLetters = new();
        CorrectAnswers = new();
    }

    public GameData(int index, LevelBlueprint blueprint)
    {
        IsInitialized = true;
        Index = index;
        CurrentLetters = blueprint.StartingLetters;
        NextLetters = blueprint.NextLetters.ToList();
        CorrectAnswers = new();
    }

    public bool IsInitialized;
    public int Index;
    public string CurrentLetters;
    public List<string> NextLetters;
    public List<string> CorrectAnswers;

    public bool UpdateLevelData(string answer)
    {
        CorrectAnswers.Add(answer);
        bool didFinish = NextLetters.Count <= 0;

        if (!didFinish)
        {
            string NextLetter = NextLetters[0];
            NextLetters.RemoveAt(0);
            CurrentLetters += NextLetter;
        }

        SaveSystem.Save(this);

        return didFinish;
    }
}


