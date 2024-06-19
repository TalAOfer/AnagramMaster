using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GameData
{
    public bool IsInitialized;

    public int OverallLevelIndex;

    public int BiomeIndex;
    public int AreaIndex;
    public int LevelIndex;

    public string CurrentLetters;
    public List<string> NextLetters;
    public List<string> CorrectAnswers;
    public bool DidFinish;

    public GameData()
    {
        IsInitialized = false;
        OverallLevelIndex = 0;
        BiomeIndex = 0;
        AreaIndex = 0;
        LevelIndex = 0;
        CurrentLetters = "";
        NextLetters = new();
        CorrectAnswers = new();
    }

    public GameData(LevelIndexHierarchy indices, LevelBlueprint blueprint)
    {
        IsInitialized = true;
        BiomeIndex = indices.Biome;
        AreaIndex = indices.Area;
        LevelIndex = indices.Level;
        CurrentLetters = blueprint.StartingLetters;
        NextLetters = blueprint.NextLetters.ToList();
        CorrectAnswers = new();
    }


    public void UpdateLevelData(string answer)
    {
        CorrectAnswers.Add(answer);
        DidFinish = NextLetters.Count <= 0;

        if (!DidFinish)
        {
            string NextLetter = NextLetters[0];
            NextLetters.RemoveAt(0);
            CurrentLetters += NextLetter;
        }

        SaveSystem.Save(this);
    }

    public LevelIndexHierarchy IndexHierarchy => new LevelIndexHierarchy(BiomeIndex, AreaIndex, LevelIndex);

}


