using Sirenix.OdinInspector;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiomeAnimalGuide : MonoBehaviour
{
    [SerializeField] List<BiomeAnimalUI> premadeAnimalImages = new();
    private readonly List<BiomeAnimalUI> activeAnimalImage = new();
    [SerializeField] private int index;

    [Button]
    public void Initialize(Biome biome)
    {
        activeAnimalImage.Clear();

        for (int animalIndex = 0; animalIndex < premadeAnimalImages.Count; animalIndex++)
        {
            bool willPremadeBeUsed = (animalIndex < biome.Areas.Count);
            BiomeAnimalUI animalUI = premadeAnimalImages[animalIndex];

            if (!willPremadeBeUsed)
            {
                animalUI.gameObject.SetActive(false);
                continue;
            }

            //Initialize premade
            animalUI.gameObject.SetActive(true);
            animalUI.SnapToState(false);
            Area currentArea = biome.Areas[animalIndex];
            animalUI.Initialize(currentArea.Animal);
        }
    }

    public void UpdateToData(GameData data)
    {
        for (int animalIndex = 0; animalIndex < activeAnimalImage.Count; animalIndex++)
        {
            BiomeAnimalUI animalUI = premadeAnimalImages[animalIndex];

            bool isBiomeFinished = data.IndexHierarchy.Biome < index;
            bool isAreaFinished = (data.IndexHierarchy.Biome == index && animalIndex < data.IndexHierarchy.Area);
            bool didAlreadyFind = isBiomeFinished || isAreaFinished;

            animalUI.SnapToState(didAlreadyFind);
        }
    }

    public IEnumerator FadeInAnimal(int index)
    {
        BiomeAnimalUI animalUI = premadeAnimalImages[index];
        yield return animalUI.FadeToColoredSequence().Play().WaitForCompletion();
    }
}

[Serializable]
public class Animal
{
    public string name;
    public string description;
    public Vector2 ImageAnchor;
    public Vector2 QuestionMarkAnchor;
    
    public Sprite SoloSpriteWithBG;
    public Sprite SoloSprite;

    public Sprite IsolatedSprite;
}
