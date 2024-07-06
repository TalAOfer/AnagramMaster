using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BankLetterContainer : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private Image image;
    public Image Image {  get { return image; } }
    [SerializeField] private RectTransform rect;
    public RectTransform Rect { get { return rect; } }

    private BankLetter _childLetter;
    public void Initialize(BankLetter letter)
    {
        _childLetter = letter;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _childLetter.OnPointerDown();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _childLetter.OnPointerEnter();
    }
}
