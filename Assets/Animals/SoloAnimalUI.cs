using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoloAnimalUI : MonoBehaviour
{
    [SerializeField] private Sprite youFoundIt;
    [SerializeField] private Sprite findTheAnimal;

    [SerializeField] private TextMeshProUGUI animalName;
    [SerializeField] private TextMeshProUGUI animalDescription;

    [SerializeField] private Image title;
    [SerializeField] private Image wholeImage;
    [SerializeField] private Image animalShadow;
    AnimationData AnimationData => AssetProvider.Instance.AnimationData;
    GameData Data => AssetProvider.Instance.Data.Value;
    BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    public void InitializeFindTheAnimal()
    {
        Animal animal = BiomeBank.GetArea(Data.IndexHierarchy).Animal;
        Initialize(animal, true);
    }
    public void Initialize(Animal animal, bool isPreLevel)
    {
        wholeImage.sprite = animal.SoloSpriteWithBG;
        
        animalShadow.sprite = animal.SoloSprite;
        animalShadow.gameObject.SetActive(isPreLevel);

        animalName.text = isPreLevel ? "?????" : animal.name;
        animalDescription.text = animal.description;
    }

    public IEnumerator FadeAnimal()
    {
        yield return AnimationData.SoloAnimalShadowFade.
            GetActionSequence(animalShadow.GetComponent<TweenableElement>()).
            Play().WaitForCompletion();
    }
}
