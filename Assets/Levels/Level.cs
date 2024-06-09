using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Level
{
    public Level(int index, LevelBlueprint blueprint)
    {
        Index = index;
        CurrentLetters = blueprint.StartingLetters;
        NextLetters = blueprint.NextLetters.ToList();
        PossibleAnswers = blueprint.PossibleAnswers.ToList();
    }

    public int Index;
    public string CurrentLetters;
    public List<string> NextLetters = new();
    public List<string> correctAnswers = new();
    public List<string> PossibleAnswers;

    public bool OnCorrectAnswer(string answer)
    {
        correctAnswers.Add(answer);

        if (NextLetters.Count > 0)
        {
            string NextLetter = NextLetters[0];
            NextLetters.RemoveAt(0);
            CurrentLetters += NextLetter;
            return false;
        } 
       
        return true;

    }
}
