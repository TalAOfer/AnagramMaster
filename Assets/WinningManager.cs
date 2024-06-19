using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinningManager : MonoBehaviour
{
    [SerializeField] private GameDataManager gameDataManager;
    [SerializeField] private ElementFader fader;
    [SerializeField] private NextLevelButton nextLevelButton;

    [FoldoutGroup("Level Bar")]
    [SerializeField] private TextMeshProUGUI levelText;
    [FoldoutGroup("Level Bar")]
    [SerializeField] private Slider slider;
    [FoldoutGroup("Level Bar")]
    [SerializeField] private List<SliderCollisionDetector> Collectibles;

    [FoldoutGroup("Biome Bar")]
    [SerializeField] private TextMeshProUGUI BiomeName;

    [FoldoutGroup("Biome Bar")]
    [Title("Biome Collectible Image")]
    [FoldoutGroup("Biome Bar")]
    [SerializeField] private Image TotalCollectibleImage;
    [FoldoutGroup("Biome Bar")]
    [SerializeField] private Tweener TotalCollectibleImageTweener;

    [FoldoutGroup("Biome Bar")]
    [Title("Biome Total Amount Text")]
    [SerializeField] private TextMeshProUGUI BiomeTotalCollectibleAmountText;

    [FoldoutGroup("Biome Bar")]
    [Title("Biome Current Amount Text")]
    [SerializeField] private TextMeshProUGUI BiomeCurrentCollectibleAmountText;
    [FoldoutGroup("Biome Bar")]
    [SerializeField] private Tweener BiomeCurrentCollectibleAmountTweener;

    private int biomeCurrentCollectibleAmount;
    private int biomeTotalCollectibleAmount;
    private NextLevelData nextLevelData;

    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;
    private GameData Data => AssetProvider.Instance.Data.Value;

    #region Initialization

    public void Initialize()
    {
        //Initialize Upper bar
        int lastWinnedStageAmount = Data.CorrectAnswers.Count;
        Biome currentBiome = BiomeBank.Biomes[Data.BiomeIndex];
        InitializeCollectibles(lastWinnedStageAmount, currentBiome);
        levelText.text = "Level " + (Data.OverallLevelIndex + 1).ToString();
        BiomeName.text = currentBiome.name;

        //Initialize Lower bar
        biomeTotalCollectibleAmount = currentBiome.GetTotalCollectibleAmount();
        biomeCurrentCollectibleAmount = currentBiome.GetCurrentCollectibleAmount(Data.IndexHierarchy);
        UpdateCollectibleCountText();

        //Check for a next-level-event (New biome? New Area?)
        nextLevelData = BiomeBank.GetNextLevelData(Data.IndexHierarchy);

        //Load next level
        gameDataManager.LoadNextLevel();
        nextLevelButton.Initialize();
    }

    private void InitializeCollectibles(int collectibleAmount, Biome currentBiome)
    {
        for (int i = 0; i < Collectibles.Count; i++)
        {
            SliderCollisionDetector collectible = Collectibles[i];
            collectible.gameObject.SetActive(i < collectibleAmount);
            collectible.Initialize(currentBiome.FullCollectible);
        }
    }

    #endregion

    public void OnButtonClicked()
    {

    }

    private void UpdateCollectibleCountText()
    {
        BiomeTotalCollectibleAmountText.text = "/" + biomeTotalCollectibleAmount.ToString();
        BiomeCurrentCollectibleAmountText.text = biomeCurrentCollectibleAmount.ToString();
    }

    public IEnumerator WinningRoutine()
    {
        yield return fader.FadeOutGameplayToWinning();

        yield return fader.FadeInGameplayToWinning();

        slider.DOValue(0.96f, AnimationData.sliderFillDuration).SetEase(AnimationData.sliderFillEase);

        switch (nextLevelData.NextLevelType)
        {
            case NextLevelEvent.None:
                break;
            case NextLevelEvent.NewArea:
                break;
            case NextLevelEvent.NewBiome:
                break;
            case NextLevelEvent.FinishedGame:
                break;
        }
    }

    public void OnCollectibleCollected()
    {
        biomeCurrentCollectibleAmount += 1;
        UpdateCollectibleCountText();
        BiomeCurrentCollectibleAmountTweener.TriggerTween(AnimationData.BiomeCurrentCollectibleAmountAnimation);

        Vector3 currentScale = TotalCollectibleImage.rectTransform.localScale;
        TotalCollectibleImage.rectTransform.localScale = currentScale * AnimationData.TotalCollectibleImageUpscaleAmount;
        TotalCollectibleImageTweener.TriggerTween(AnimationData.TotalCollectibleImageAnimation);
    }
}
