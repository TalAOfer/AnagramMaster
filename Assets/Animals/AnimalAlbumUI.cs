using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalAlbumUI : MonoBehaviour
{
    [SerializeField] List<AnimalAlbumCardContainer> containers = new();
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;

    [Button]
    public void Initialize(Biome biome, int currentAreaIndex)
    {
        for (int animalIndex = 0; animalIndex < containers.Count; animalIndex++)
        {
            AnimalAlbumCardContainer currentContainer = containers[animalIndex];
            bool doesAlreadyHaveAnimal = (animalIndex < currentAreaIndex);

            if (doesAlreadyHaveAnimal)
            {
                currentContainer.ChildUI.Initialize(biome.Areas[animalIndex].Animal);
                currentContainer.ChildUI.SetExplicitAnimalData();
                currentContainer.ChildUI.gameObject.SetActive(true);
            }

            else
            {
                currentContainer.ChildUI.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator FadeInAnimal(int index)
    {
        SoloAnimalUI animalUI = containers[index].ChildUI;
        if (animalUI.gameObject.TryGetComponent<TweenableElement>(out var animalUIElement))
        {
            yield return AnimationData.AnimalAlbumPhotoFadeIn.GetActionSequence(animalUIElement).Play().WaitForCompletion();
        }

        else yield break;
    }

    //public void UpdateToData(GameData data)
    //{
    //    for (int animalIndex = 0; animalIndex < activeAnimalImage.Count; animalIndex++)
    //    {
    //        BiomeAnimalUI animalUI = premadeAnimalImages[animalIndex];

    //        bool isBiomeFinished = data.IndexHierarchy.Biome < index;
    //        bool isAreaFinished = (data.IndexHierarchy.Biome == index && animalIndex < data.IndexHierarchy.Area);
    //        bool didAlreadyFind = isBiomeFinished || isAreaFinished;

    //        //animalUI.SnapToState(didAlreadyFind);
    //    }
    //}


}

[Serializable]
public class Animal
{
    public string name;
    public string description;

    public Sprite ShownSprite;
    public Sprite HiddenSprite;
}
