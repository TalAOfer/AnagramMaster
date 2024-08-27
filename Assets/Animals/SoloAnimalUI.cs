using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoloAnimalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI animalName;
    [SerializeField] private Image image;
    [SerializeField] private Image overlay;
    [SerializeField] private TweenableElement element;
    public TweenableElement Element => element;

    private TweenableElement _overlayElement;
    private TweenableElement OverlayElement
    {
        get
        {
            if (_overlayElement == null)
            {
                _overlayElement = overlay.GetComponent<TweenableElement>();
            }
            return _overlayElement;
        }
    }

    private Animal _animal;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;

    public void Initialize(Animal animal)
    {
        _animal = animal;
        image.sprite = _animal.HiddenSprite;
        animalName.text = "?????";
        overlay.color = Tools.Transparent();
    }

    public IEnumerator FlashAndReveal()
    {
        overlay.gameObject.SetActive(true);
        overlay.color = Color.white;
        overlay.GetComponent<CanvasGroup>().alpha = 1.0f;

        SetExplicitAnimalData();

        yield return AnimationData.SoloAnimalFlashFade.PlayAndWait(OverlayElement);
    }

    public void SetExplicitAnimalData()
    {
        image.sprite = _animal.ShownSprite;
        animalName.text = _animal.name;
    }
}
