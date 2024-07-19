using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoloAnimalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI animalName;
    [SerializeField] private TextMeshProUGUI animalDescription;
    [SerializeField] private Image animalImage;
    public void Initialize(Animal animal)
    {
        animalImage.sprite = animal.SpriteWithBG;
        animalName.text = animal.name;
        animalDescription.text = animal.description;
    }
}
