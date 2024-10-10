using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameEvent OnDataInitialized;
    private GiftBank GiftBank => AssetProvider.Instance.GiftBank;
    private CurrentData Data => AssetProvider.Instance.Data;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;


    public void Start()
    {
        InitializeData();
        DOTween.SetTweensCapacity(200, 125);
    }

    public void InitializeData()
    {
        Data.Value = SaveSystem.Load();

        bool isFirstStage = !Data.Value.IsInitialized;
        if (isFirstStage)
        {
            InitializeNewGameData(new LevelIndexHierarchy(0, 0, 0), 0);
            Data.Value.IsInitialized = true;
        }

        else if (Data.Value.Level.DidFinish)
        {
            NextLevelData nextLevelData = new(Data.Value.IndexHierarchy, BiomeBank);
            if (nextLevelData.NextLevelEvent != NextLevelEvent.FinishedGame)
            {
                LoadNextLevel();
            }
        }

        OnDataInitialized.Raise();
    }

    public void SaveNewData()
    {
        SaveSystem.Save(Data.Value);
    }

    private void InitializeNewGameData(LevelIndexHierarchy indexHierarchy, int levelIndex)
    {
        Data.Value = new GameData(indexHierarchy)
        {
            OverallLevelIndex = levelIndex,
            Gift = new Gift(GiftBank.Blueprints[0])
        };

        LevelBlueprint levelBlueprint = BiomeBank.GetLevel(indexHierarchy);
        InitializeLevel(indexHierarchy, levelBlueprint);
    }

    public void LoadNextLevel()
    {
        NextLevelData nextLevelData = new(Data.Value.IndexHierarchy, BiomeBank);

        if (Data.Value.GiftTypeIndex >= GiftBank.Blueprints.Count)
        {
            Data.Value.GiftTypeIndex = 0;
        }

        if (nextLevelData.NextLevelEvent == NextLevelEvent.FinishedGame) return;

        Data.Value.IncrementProgression(GiftBank.Blueprints[Data.Value.GiftTypeIndex]);

        LevelBlueprint nextLevelBlueprint = BiomeBank.GetLevel(nextLevelData.IndexHierarchy);
        InitializeLevel(nextLevelData.IndexHierarchy, nextLevelBlueprint);
    }

    private void InitializeLevel(LevelIndexHierarchy indexHierarchy, LevelBlueprint blueprint)
    {
        Data.Value.InitializeNewLevel(blueprint, indexHierarchy);
        SaveNewData();
    }

    #region Helpers

    [Button]
    public void ResetData()
    {
        InitializeNewGameData(new LevelIndexHierarchy(0, 0, 0), 0);
    }

    [Button]
    public void SetToLevel(int index)
    {
        int levelIndex = index - 1;
        LevelIndexHierarchy indexHierarchy = BiomeBank.GetLevelIndexHierarchyWithTotalIndex(levelIndex);
        InitializeNewGameData(indexHierarchy, levelIndex);
        Data.Value.IsInitialized = true;
        SaveNewData();
    }

    #endregion

}
