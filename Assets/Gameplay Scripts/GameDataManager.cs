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
        Data.Value = new GameData();
        SaveNewData();
    }

    public void Start()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        Data.Value = SaveSystem.Load();

        bool isFirstStage = !Data.Value.IsInitialized;
        if (isFirstStage)
        {
            InitializeFirstLevel();
        }

        else if (Data.Value.Level.DidFinish)
        {
            NextLevelData nextLevelData = new(Data.Value.IndexHierarchy, BiomeBank);
            if (nextLevelData.NextLevelEvent is not NextLevelEvent.FinishedGame)
            {
                LoadNextLevel();
            }
        }

        OnDataInitialized.Raise();
    }

    private void InitializeFirstLevel()
    {
        LevelIndexHierarchy indexHierarchy = new LevelIndexHierarchy(0, 0, 0);
        LevelBlueprint levelBlueprint = BiomeBank.GetLevel(indexHierarchy);
        Data.Value = new GameData(indexHierarchy);
        Data.Value.InitializeNewLevel(levelBlueprint);
        Data.Value.IsInitialized = true;
        SaveNewData();
    }

    public void LoadNextLevel()
    {
        Data.Value.IncrementProgression();
        NextLevelData nextLevelData = new(Data.Value.IndexHierarchy, BiomeBank);
        if (nextLevelData.NextLevelEvent is not NextLevelEvent.FinishedGame)
        {
            LevelBlueprint nextLevelBlueprint = BiomeBank.GetLevel(nextLevelData.IndexHierarchy);
            Data.Value.InitializeNewLevel(nextLevelBlueprint);
            SaveNewData();
        }
    }

    public void SaveNewData()
    {
        SaveSystem.Save(Data.Value);
    }

    [Button]
    public void SetToLevel(int index)
    {
        int levelIndex = index - 1;
        LevelIndexHierarchy indicesHierarchy = BiomeBank.GetLevelIndexHierarchyWithTotalIndex(levelIndex);
        LevelBlueprint levelBlueprint = BiomeBank.GetLevel(indicesHierarchy);
        Data.Value = new GameData(indicesHierarchy);
        Data.Value.InitializeNewLevel(levelBlueprint);
        SaveNewData();
    }
}
