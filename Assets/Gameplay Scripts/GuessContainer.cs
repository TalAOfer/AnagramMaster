using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessContainer : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    public RectTransform Rect {  get { return rect; } }
    [SerializeField] private Image image;
    public Image Image { get { return image; } }

    [SerializeField] private Color defaultContainerColor;
    [SerializeField] private Sprite defaultContainerSprite; 

    [SerializeField] private Sprite filledContainerSprite;
    [SerializeField] private Color filledContainerColor;

    public void SetVisualToDefault()
    {
        Image.sprite = defaultContainerSprite;
        Image.color = defaultContainerColor;
    }

    public void SetVisualToFull()
    {
        Image.sprite = filledContainerSprite;
        Image.color = filledContainerColor;
    }
}
