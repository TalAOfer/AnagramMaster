using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    public TextMeshProUGUI Tmp { get { return tmp; } private set { } }
    [SerializeField] private RectTransform rect;
    public RectTransform Rect { get { return rect; } private set { } }
    public bool IsUsed {  get; private set; }
    private GuessLetter originalParentLetter;

    public void SetUsed(bool isUsed) 
    {
        if (IsUsed != isUsed)
        {
            IsUsed = isUsed;
            gameObject.SetActive(isUsed);
        }

        if (!isUsed)
        {
            rect.SetParent(originalParentLetter.Rect);
        }
    }

    public void Initialize(GuessLetter parentLetter)
    {
        originalParentLetter = parentLetter;
        Tmp.text = parentLetter.Tmp.text;
    }

    public void ResetTransformToZero()
    {
        // Reset the scale and rotation to match the parent
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;

        // Center the RectTransform
        rect.anchoredPosition = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

}
