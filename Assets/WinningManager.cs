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
    [SerializeField] private TextMeshProUGUI extraText;

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

    [FoldoutGroup("New Area Image")]
    [SerializeField] private RectTransform NewAreaContainer;
    [FoldoutGroup("New Area Image")]
    [SerializeField] private Image NewAreaImage;
    [FoldoutGroup("New Area Image")]
    [SerializeField] private Image NewAreaContainerImage;

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
        slider.value = 0;
        int lastWinnedStageAmount = Data.CorrectAnswers.Count;
        Biome currentBiome = BiomeBank.Biomes[Data.BiomeIndex];
        InitializeCollectibles(lastWinnedStageAmount, currentBiome);
        levelText.text = "Level " + (Data.OverallLevelIndex + 1).ToString();
        BiomeName.text = currentBiome.name;

        //Initialize Lower bar
        TotalCollectibleImage.rectTransform.localScale = Vector3.one;
        biomeTotalCollectibleAmount = currentBiome.GetTotalCollectibleAmount();
        biomeCurrentCollectibleAmount = currentBiome.GetCurrentCollectibleAmount(Data.IndexHierarchy);
        UpdateCollectibleCountText();

        //Check for a next-level-event (New biome? New Area?)
        nextLevelData = BiomeBank.GetNextLevelData(Data.IndexHierarchy);
        nextLevelButton.Initialize(nextLevelData);

        //Load next level
        if (nextLevelData.NextLevelType is not NextLevelEvent.FinishedGame)
        {
            gameDataManager.LoadNextLevel();
        }
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

        SoundManager.PlaySound("WinningCollectibleBarFill", Vector3.zero);
        yield return slider.DOValue(0.96f, AnimationData.sliderFillDuration).SetEase(AnimationData.sliderFillEase).WaitForCompletion();
        BiomeArea area = BiomeBank.GetArea(Data.IndexHierarchy);
        LevelBlueprint level = BiomeBank.GetLevel(Data.IndexHierarchy);

        switch (nextLevelData.NextLevelType)
        {
            case NextLevelEvent.None:
                yield return fader.PlayRegularEndOfWinningSequence();
                break;
            case NextLevelEvent.NewArea:
                fader.CurrentInactiveGameplayBackground.sprite = area.Sprite;
                NewAreaContainer.localScale = Vector3.one;
                NewAreaImage.sprite = area.Sprite;
                NewAreaContainerImage.color = level.containerBG;
                extraText.text = "New Area Unlocked!";
                yield return fader.PlayNewAreaWinningSequence();
                break;
            case NextLevelEvent.NewBiome:
                fader.CurrentInactiveGameplayBackground.sprite = area.Sprite;
                NewAreaContainer.localScale = Vector3.one * AnimationData.NewBiomeImageUpscale;
                NewAreaImage.sprite = area.Sprite;
                NewAreaContainerImage.color = level.containerBG;
                NewAreaImage.sprite = BiomeBank.GetArea(Data.IndexHierarchy).Sprite;
                extraText.text = "New Destination Unlocked!";
                yield return fader.PlayNewBiomeWinningSequence();
                break;
            case NextLevelEvent.FinishedGame:
                extraText.text = "You finished the game, \n New levels soon!";
                yield return fader.PlayFinishedGameWinningSequence();
                break;
        }
    }

    public void OnCollectibleCollected()
    {
        biomeCurrentCollectibleAmount += 1;
        SoundManager.PlaySound("WinningCollectibleCollected", Vector3.zero);
        UpdateCollectibleCountText();
        BiomeCurrentCollectibleAmountTweener.TriggerTween(AnimationData.BiomeCurrentCollectibleAmountAnimation);

        Vector3 currentScale = TotalCollectibleImage.rectTransform.localScale;
        TotalCollectibleImage.rectTransform.localScale = currentScale * AnimationData.TotalCollectibleImageUpscaleAmount;
        TotalCollectibleImageTweener.TriggerTween(AnimationData.TotalCollectibleImageAnimation);
    }
}
