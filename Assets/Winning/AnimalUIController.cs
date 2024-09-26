using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI animalProgressionText;
    [SerializeField] private Slider animalSlider;
    private Vector2Int AnimalCount;

    [SerializeField] private AnimalAlbumUI AnimalAlbumUI;
    [SerializeField] private SoloAnimalUI SoloAnimalUI;
    [SerializeField] private Image Title;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;

    private float GetAnimalSliderFill() => (float)AnimalCount.x / AnimalCount.y;
    private string GetAnimalAmountString() => AnimalCount.x.ToString() + "/" + AnimalCount.y.ToString();

    private GameData _data;

    public void Initialize(GameData data)
    {
        _data = data;
        InitializeAnimalBar();

        Animal animal = BiomeBank.GetAnimal(_data.IndexHierarchy);
        SoloAnimalUI.Initialize(animal);
        Biome biome = BiomeBank.Biomes[data.IndexHierarchy.Biome];
        AnimalAlbumUI.Initialize(biome, data.IndexHierarchy.Area);
        Title.sprite = AnimationData.YouFoundItSprite;
    }

    private void InitializeAnimalBar()
    {
        int currentLevelIndex = _data.IndexHierarchy.Level;
        int currentAreaLevelAmount = BiomeBank.GetArea(_data.IndexHierarchy).Levels.Count;
        AnimalCount = new(currentLevelIndex, currentAreaLevelAmount);
        animalProgressionText.text = GetAnimalAmountString();
        animalSlider.value = (float)AnimalCount.x / AnimalCount.y;
    }

    public IEnumerator AnimalRoutine()
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

        yield return AnimationData.Black_Overlay_In.PlayAndWait();

        yield return AnimationData.Animal_Fade_In.PlayAndWait();

        yield return SoloAnimalUI.FlashAndReveal();

        yield return AnimationData.Animal_Album_In.PlayAndWait();

        yield return AnimationData.Animal_Fade_Out.PlayAndWait();

        Title.sprite = AnimationData.FindTheAnimalSprite;

        yield return AnimalAlbumUI.FadeInAnimal(_data.IndexHierarchy.Area);

        bool isLastArea = _data.IndexHierarchy.Area == 3;

        if (isLastArea)
        {
            yield return AnimalAlbumUI.AnimateFinishedAlbum();
        }

        yield return AnimationData.Animal_Album_Out.PlayAndWait();

        yield return AnimationData.Black_Overlay_Out.PlayAndWait();
    }

}
