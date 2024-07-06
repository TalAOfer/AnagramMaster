using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinningManager : MonoBehaviour
{
    [SerializeField] private StartMenuManager startMenuManager;
    [SerializeField] private ElementController fader;
    [SerializeField] private WinningButton nextLevelButton;
    [SerializeField] private TextMeshProUGUI extraText;
    [SerializeField] private Color nativePanelTextColor;

    [FoldoutGroup("Gift Bar")]
    [SerializeField] private TextMeshProUGUI giftProgressionText;
    [FoldoutGroup("Gift Bar")]
    [SerializeField] private Slider giftSlider;
    private Vector2Int GiftCount;

    [FoldoutGroup("Animal Bar")]
    [SerializeField] private TextMeshProUGUI animalProgressionText;
    [FoldoutGroup("Animal Bar")]
    [SerializeField] private Slider animalSlider;
    private Vector2Int AnimalCount;

    private NextLevelData nextLevelData;

    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;
    private GameData Data => AssetProvider.Instance.Data.Value;
    private GameData _dataClone;

    private float GetAnimalSliderFill() => (float)AnimalCount.x / AnimalCount.y;
    private float GetGiftSliderFill() => (float)GiftCount.x / GiftCount.y;
    private string GetGiftAmountString() => GiftCount.x.ToString() + "/" + GiftCount.y.ToString();
    private string GetAnimalAmountString() => AnimalCount.x.ToString() + "/" + AnimalCount.y.ToString();

    #region Initialization

    public void Initialize(GameData dataClone)
    {
        _dataClone = dataClone;

        //Initialize Upper bar
        InitializeGiftBar();
        InitializeAnimalBar();
        
        //Check for a next-level-event (New biome? New Area?)
        NextLevelData nextLevelData = new(Data.IndexHierarchy, BiomeBank);
        nextLevelButton.Initialize(nextLevelData);
    }


    #endregion

    #region Gift Prgoression
    private void InitializeGiftBar()
    {
        int currentGiftAmount = _dataClone.GiftProgressionAmount;
        int targetGiftAmount = _dataClone.GiftTargetAmount;
        GiftCount = new(currentGiftAmount, targetGiftAmount);
        giftSlider.value = GetGiftSliderFill();
        giftProgressionText.text = GetGiftAmountString();
    }
    private IEnumerator GiftRoutine()
    {
        GiftCount.x += 1;
        SoundManager.PlaySound("WinningCollectibleBarFill", Vector3.zero);
        Tween tween = giftSlider.DOValue(GetAnimalSliderFill(), AnimationData.sliderFillDuration).SetEase(AnimationData.sliderFillEase);

        tween.OnUpdate(() =>
        {
            if (giftSlider.value >= 0.48f)
            {
                giftProgressionText.text = GetGiftAmountString();
                tween.OnUpdate(null);
            }
        });

        yield return tween.WaitForCompletion();

        if (GiftCount.x == GiftCount.y)
        {
            yield return OpenGift();
        }
    }

    private IEnumerator OpenGift()
    {
        Debug.Log("You got the gift!");
        yield return null;
    }

    #endregion

    #region Animal Tracking Progression

    private void InitializeAnimalBar()
    {
        int currentAreaIndex = _dataClone.IndexHierarchy.Area;
        int currentBiomeAreaAmount = BiomeBank.Biomes[_dataClone.IndexHierarchy.Biome].Areas.Count;
        AnimalCount = new(currentAreaIndex, currentBiomeAreaAmount);
        animalProgressionText.text = GetAnimalAmountString();
        animalSlider.value = (float)AnimalCount.x / AnimalCount.y;
    }

    private IEnumerator AnimalRoutine()
    {
        AnimalCount.x += 1;
        SoundManager.PlaySound("WinningCollectibleBarFill", Vector3.zero);
        Tween tween = animalSlider.DOValue(GetAnimalSliderFill(), AnimationData.sliderFillDuration).SetEase(AnimationData.sliderFillEase);

        tween.OnUpdate(() =>
        {
            if (animalSlider.value >= 0.48f)
            {
                animalProgressionText.text = GetAnimalAmountString();
                tween.OnUpdate(null);
            }
        });

        yield return tween.WaitForCompletion();

        if (AnimalCount.x == AnimalCount.y)
        {
            yield return OpenGift();
        }
    }

    private IEnumerator ShowAnimal()
    {
        Debug.Log("Wow, Animal!");
        yield break;
    }

    #endregion



    public IEnumerator WinningRoutine()
    {
        yield return fader.FadeOutGameplayToWinning();

        yield return fader.FadeInGameplayToWinning();

        yield return GiftRoutine();

        yield return AnimalRoutine();

        //Area area = BiomeBank.GetArea(Data.IndexHierarchy);

        //switch (nextLevelData.NextLevelEvent)
        //{
        //    case NextLevelEvent.None:
        //        yield return fader.PlayRegularEndOfWinningSequence();
        //        break;
        //    case NextLevelEvent.NewArea:
        //        //fader.CurrentInactiveGameplayBackground.sprite = area.Sprite;
        //        NewAreaContainer.localScale = Vector3.one;
        //        NewAreaImage.sprite = area.Sprite;
        //        NewAreaContainerImage.color = nativePanelTextColor;
        //        extraText.text = "New Area Unlocked!";
        //        extraText.color = nativePanelTextColor;
        //        yield return fader.PlayNewAreaWinningSequence();
        //        break;
        //    case NextLevelEvent.NewBiome:
        //        //fader.CurrentInactiveGameplayBackground.sprite = area.Sprite;
        //        NewAreaContainer.localScale = Vector3.one * AnimationData.NewBiomeImageUpscale;
        //        NewAreaImage.sprite = area.Sprite;
        //        NewAreaContainerImage.color = area.LetterContainerBGColor;
        //        NewAreaImage.sprite = BiomeBank.GetArea(Data.IndexHierarchy).Sprite;
        //        extraText.text = "New Destination Unlocked";
        //        extraText.color = Color.white;
        //        yield return fader.PlayNewBiomeWinningSequence();
        //        break;
        //    case NextLevelEvent.FinishedGame:
        //        startMenuManager.Initialize();
        //        extraText.text = "Thank you for playing!";
        //        extraText.color = nativePanelTextColor;
        //        yield return fader.PlayFinishedGameWinningSequence();
        //        break;
        //}
    }

    


    
}
