using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiomeAnimalGuide : MonoBehaviour
{
    [SerializeField] List<BiomeAnimalUI> premadeAnimalImages = new();
    private readonly List<BiomeAnimalUI> activeAnimalImage = new();
    [SerializeField] private int index;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    public void Initialize(GameData Data)
    {
        activeAnimalImage.Clear();

        for (int animalIndex = 0; animalIndex < premadeAnimalImages.Count; animalIndex++)
        {
            Biome currentBiome = BiomeBank.Biomes[Data.IndexHierarchy.Biome];
            bool willPremadeBeUsed = (animalIndex < currentBiome.Areas.Count);
            BiomeAnimalUI animalUI = premadeAnimalImages[animalIndex];

            if (!willPremadeBeUsed)
            {
                animalUI.gameObject.SetActive(false);
                continue;
            }

            //Initialize premade
            animalUI.gameObject.SetActive(true);
            Area currentArea = BiomeBank.GetArea(Data.IndexHierarchy);

            animalUI.Initialize(currentArea.Animal);

            bool isBiomeFinished = Data.IndexHierarchy.Biome < index;
            bool isAreaFinished = (Data.IndexHierarchy.Biome == index && animalIndex < Data.IndexHierarchy.Area);
            bool didAlreadyFind = isBiomeFinished || isAreaFinished;

            animalUI.SnapToState(didAlreadyFind);
        }
    }

    public void FadeNextAnimal()
    {

    }
}

[Serializable]
public class Animal
{
    public string name;
    public string description;
    public Vector2 ImageAnchor;
    public Vector2 QuestionMarkAnchor;
    public Sprite SpriteWithBG;
    public Sprite IsolatedSprite;
}
