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
    [SerializeField] private Tweener tweener;
    public Tweener Tweener { get {  return tweener; } }
    [SerializeField] private TweenBlueprint correctAnswerAnim;

    [SerializeField] private Color defaultContainerColor;
    [SerializeField] private Sprite defaultContainerSprite; 

    [SerializeField] private Sprite filledContainerSprite;
    private Color _filledContainerColor;

    public void Initialize(Color color)
    {
        _filledContainerColor = color;
    }

    public void SetVisualToDefault()
    {
        Image.sprite = defaultContainerSprite;
        Image.color = defaultContainerColor;
    }

    public void SetVisualToFull()
    {
        Image.sprite = filledContainerSprite;
        Image.color = _filledContainerColor;
    }

    public void StartCorrectAnswerAnimation()
    {
        tweener.TriggerTween(correctAnswerAnim);
    }
}
