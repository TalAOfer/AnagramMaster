using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameEvent OnDataInitialized;

    private CurrentData Data => AssetProvider.Instance.Data;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;

    [Button]
    public void ResetData()
    {
        GameData data = new();
        SaveNewData(data);
    }

    public void Awake()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        Data.Value = SaveSystem.Load();

        bool isFirstStage = !Data.Value.IsInitialized;
        if (isFirstStage)
        {
            LevelIndexHierarchy indexHierarchy = new LevelIndexHierarchy(0, 0, 0);
            Data.Value = new GameData(indexHierarchy, BiomeBank.GetLevel(indexHierarchy));
            SaveNewData(Data.Value);
        }

        else if (Data.Value.DidFinish)
        {
            NextLevelData nextLevelData = BiomeBank.GetNextLevelData(Data.Value.IndexHierarchy);
            if (nextLevelData.NextLevelType is not NextLevelEvent.FinishedGame)
            {
                LoadNextLevel();
            }
        }

        OnDataInitialized.Raise();
    }

    public void LoadNextLevel()
    {
        int currentOverallLevel = Data.Value.OverallLevelIndex;
        NextLevelData nextLevelData = BiomeBank.GetNextLevelData(Data.Value.IndexHierarchy);
        LevelBlueprint nextLevelBlueprint = BiomeBank.GetLevel(nextLevelData.LevelIndexHierarchy);
        GameData newGameData = new(nextLevelData.LevelIndexHierarchy, nextLevelBlueprint)
        {
            OverallLevelIndex = currentOverallLevel + 1
        };

        SaveNewData(newGameData);
    }

    public void SaveNewData(GameData newGameData)
    {
        SaveSystem.Save(newGameData);
        Data.Value = newGameData;
    }
}
