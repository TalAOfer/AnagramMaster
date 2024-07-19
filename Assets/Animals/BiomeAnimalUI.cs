using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiomeAnimalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionMark;
    [SerializeField] private Image animalImage;

    public void Initialize(Animal animal)
    {
        animalImage.sprite = animal.IsolatedSprite;
        animalImage.rectTransform.anchoredPosition = animal.ImageAnchor;
        questionMark.rectTransform.anchoredPosition = animal.QuestionMarkAnchor;
    }

    public void SnapToState(bool visible)
    {
        questionMark.color = visible? Tools.Transparent() : Color.white;
        animalImage.color = visible? Color.white : Color.black;
    }

    public Sequence FadeToColoredSequence()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(questionMark.DOFade(0, 1));
        sequence.Join(animalImage.DOColor(Color.white, 1));
        return sequence;
    }
}
