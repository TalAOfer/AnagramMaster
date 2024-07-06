using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GameData
{
    public bool IsInitialized;

    public int OverallLevelIndex;
    public LevelData Level;
    public LevelIndexHierarchy IndexHierarchy;

    public int HintAmount;

    public int GiftProgressionAmount;
    public int GiftTargetAmount { get; private set; } = 3;

    public GameData()
    {
        IsInitialized = false;
        Level = null;

        OverallLevelIndex = 0;
        GiftProgressionAmount = 0;
        HintAmount = 0;
    }

    public GameData(LevelIndexHierarchy indices) 
    {
        IndexHierarchy = indices;
    }


    public void InitializeNewLevel(LevelBlueprint blueprint)
    {
        Level = new LevelData(blueprint);
    }

    public void ChangeHintAmount(int increment)
    {
        HintAmount += increment;
        
        if (HintAmount < 0) 
        {
            HintAmount = 0; 
        }
    }

    public void OnCorrectAnswer(string answer)
    {
        Level.UpdateLevelState(answer);
        SaveSystem.Save(this);
    }

    public void IncrementProgression()
    {
        GiftProgressionAmount += 1;
        if (GiftProgressionAmount >= GiftTargetAmount)
        {
            GetGift(Level.Gift);
            GiftProgressionAmount = 0;
        }
    }
    
    private void GetGift(Gift gift)
    {
        switch (gift.GiftType)
        {
            case GiftType.Hint:
                ChangeHintAmount(gift.GiftAmount);
                break;
        }
    }

    public GameData Clone()
    {
        return new GameData
        {
            IsInitialized = this.IsInitialized,
            OverallLevelIndex = this.OverallLevelIndex,
            Level = this.Level?.Clone(),
            IndexHierarchy = this.IndexHierarchy, // Assuming LevelIndexHierarchy is a struct or an immutable class
            HintAmount = this.HintAmount,
            GiftProgressionAmount = this.GiftProgressionAmount
        };
    }
}

[System.Serializable]
public class LevelData
{
    public string CurrentLetters;
    public bool[] HintState;
    public List<string> NextLetters;
    public List<string> CorrectAnswers;
    public bool DidFinish;
    public Gift Gift;
    public LevelData(LevelBlueprint blueprint)
    {
        DidFinish = false;
        CurrentLetters = blueprint.StartingLetters;
        NextLetters = blueprint.NextLetters.ToList();
        CorrectAnswers = new();
        Gift = new();
        HintState = new bool[CurrentLetters.Length];
    }

    public bool UpdateLevelState(string answer)
    {
        CorrectAnswers.Add(answer);
        DidFinish = NextLetters.Count <= 0;

        if (!DidFinish)
        {
            string NextLetter = NextLetters[0];
            NextLetters.RemoveAt(0);
            CurrentLetters += NextLetter;
            HintState = new bool[CurrentLetters.Length];
        } 

        return DidFinish;
    }

    public LevelData Clone()
    {
        return new LevelData
        {
            CurrentLetters = this.CurrentLetters,
            HintState = (bool[])this.HintState.Clone(),
            NextLetters = new List<string>(this.NextLetters),
            CorrectAnswers = new List<string>(this.CorrectAnswers),
            DidFinish = this.DidFinish,
            Gift = this.Gift.Clone()
        };
    }

    private LevelData() { }
}


