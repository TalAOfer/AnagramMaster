using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinningManager : MonoBehaviour
{
    [SerializeField] private GiftUI GiftUI; 
    [SerializeField] private StartMenuManager startMenuManager;
    [SerializeField] private WinningButton nextLevelButton;
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

    [SerializeField] private BiomeAnimalGuide BiomeAnimalGuide;
    [SerializeField] private SoloAnimalUI SoloAnimalUI;

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

        InitializeGiftBar();
        InitializeAnimalBar();

        Animal animal = BiomeBank.GetArea(dataClone.IndexHierarchy).Animal;
        SoloAnimalUI.Initialize(animal, false);
        //BiomeAnimalGuide.Initialize(BiomeBank.Biomes[dataClone.IndexHierarchy.Biome]);

        NextLevelData nextLevelData = new(Data.IndexHierarchy, BiomeBank);
        nextLevelButton.Initialize(nextLevelData);
    }

    public IEnumerator WinningRoutine()
    {
        yield return AnimationData.G_Fade_Out.PlayAndWait();

        yield return AnimationData.W_Fade_In.PlayAndWait();

        yield return GiftRoutine();

        yield return AnimalRoutine();

        AnimationData.W_Button_Fade_In.Play();
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
        SoundManager.PlaySound("GiftBarFill", Vector3.zero);
        Tween tween = giftSlider.DOValue(GetGiftSliderFill(), AnimationData.sliderFillDuration).SetEase(AnimationData.sliderFillEase);
        StartCoroutine(CoroutineRunner.Instance.RunFunctionDelayed
            (AnimationData.sliderFillDuration / 2f, 
            () => giftProgressionText.text = GetGiftAmountString()
        ));

        yield return tween.WaitForCompletion();

        if (GiftCount.x == GiftCount.y)
        {
            yield return OpenGift();
        }
    }

    private IEnumerator OpenGift()
    {
        GiftUI.Initialize(_dataClone.Gift);
        yield return GiftUI.OpenGiftAnimation();
    }

    #endregion

    #region Animal Tracking Progression

    private void InitializeAnimalBar()
    {
        int currentLevelIndex = _dataClone.IndexHierarchy.Level;
        int currentAreaLevelAmount = BiomeBank.GetArea(_dataClone.IndexHierarchy).Levels.Count;
        AnimalCount = new(currentLevelIndex, currentAreaLevelAmount);
        animalProgressionText.text = GetAnimalAmountString();
        animalSlider.value = (float)AnimalCount.x / AnimalCount.y;
    }

    private IEnumerator AnimalRoutine()
    {
        AnimalCount.x += 1;
        SoundManager.PlaySound("AnimalBarFill", Vector3.zero);
        Tween tween = animalSlider.DOValue(GetAnimalSliderFill(), AnimationData.sliderFillDuration).SetEase(AnimationData.sliderFillEase);

        StartCoroutine(CoroutineRunner.Instance.RunFunctionDelayed
            (AnimationData.sliderFillDuration / 2f,
            () => animalProgressionText.text = GetAnimalAmountString()
        ));

        yield return tween.WaitForCompletion();

        if (AnimalCount.x == AnimalCount.y)
        {
            yield return ShowAnimal();
        }
    }

    private IEnumerator ShowAnimal()
    {

        yield return AnimationData.W_Fade_Out.PlayAndWait();
        
        yield return AnimationData.Animal_Fade_In.PlayAndWait();

        yield return SoloAnimalUI.FadeAnimal();

        //yield return BiomeAnimalGuide.FadeInAnimal(_dataClone.IndexHierarchy.Area);

        //yield return AnimationData.Animal_Fade_Out.PlayAndWait();
    }

    #endregion
}
