using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftItemUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rect;
    [SerializeField] private TextMeshProUGUI amountText;
    public RectTransform Rect { get { return rect; } }

    public void Initialize(Sprite sprite, int amount)
    {
        image.sprite = sprite;
        amountText.text = "X" + amount.ToString();
    }
}
