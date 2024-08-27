using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnimalAlbumUI : MonoBehaviour
{
    [SerializeField] private List<AnimalAlbumCardContainer> containers = new();
    [SerializeField] private TweenableElement textElement;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;

    [Button]
    public void Initialize(Biome biome, int currentAreaIndex)
    {
        for (int animalIndex = 0; animalIndex < containers.Count; animalIndex++)
        {
            AnimalAlbumCardContainer currentContainer = containers[animalIndex];
            bool doesAlreadyHaveAnimal = (animalIndex < currentAreaIndex);

            currentContainer.ChildUI.Initialize(biome.Areas[animalIndex].Animal);
            currentContainer.ChildUI.SetExplicitAnimalData();

            if (doesAlreadyHaveAnimal)
            {
                currentContainer.ChildUI.gameObject.SetActive(true);
                currentContainer.ChildUI.GetComponent<CanvasGroup>().alpha = 1f;
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
            yield return AnimationData.AnimalAlbumPhotoFadeIn.PlayAndWait(animalUIElement);
        }

        else yield break;
    }

    public IEnumerator AnimateFinishedAlbum()
    {
        List<TweenableElement> containerElements = containers
       .Select(container => container.ChildUI.Element)
       .ToList();

        yield return AnimationData.CorrectGuessAnim.PlayAndWait(containerElements);
        yield return AnimationData.Animal_Album_Text_In.PlayAndWait(textElement);
        yield return AnimationData.Animal_Album_Text_Out.PlayAndWait(textElement);
    }
}

[Serializable]
public class Animal
{
    public string name;
    public string description;

    public Sprite ShownSprite;
    public Sprite HiddenSprite;
}
