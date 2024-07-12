using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiomeAnimalGuide : MonoBehaviour
{
    [SerializeField] List<Image> animalImages = new();
    [SerializeField] private int index;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    public void Initialize(GameData Data)
    {
        for (int animalIndex = 0; animalIndex < animalImages.Count; animalIndex++)
        {
            var image = animalImages[animalIndex];
            Area currentArea = BiomeBank.GetArea(Data.IndexHierarchy);
            bool isBiomeFinished = Data.IndexHierarchy.Biome < index;
            bool isAreaFinished = (Data.IndexHierarchy.Biome == index && animalIndex < Data.IndexHierarchy.Area);
            
            if (isBiomeFinished || isAreaFinished)
            {
                Sprite coloredSprite = currentArea.Animal.IsolatedColoredSprite;
                image.sprite = coloredSprite;
            }

            else
            {
                Sprite blackSprite = currentArea.Animal.IsolatedBlackSprite;
                image.sprite = blackSprite;
            }
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
    public Sprite SpriteWithBG;
    public Sprite IsolatedColoredSprite;
    public Sprite IsolatedBlackSprite;
}
