using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiomeAnimalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionMark;
    [SerializeField] private Image animalImage;

    public void FadeToColored()
    {
        questionMark.DOFade(0, 1);
        animalImage.DOColor(Color.white, 1);
    }
}
