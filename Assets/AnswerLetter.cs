using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class AnswerLetter : MonoBehaviour
{
    public TextMeshProUGUI Tmp {  get; private set; }
    public BankLetter OriginalParentLetter {  get; private set; }
    public AnswerContainer CurrentAnswerContainer { get; private set; }
    public RectTransform RectTransform { get; private set; }

    public void Initialize(BankLetter parentLetter)
    {
        RectTransform = GetComponent<RectTransform>();  
        Tmp = GetComponentInChildren<TextMeshProUGUI>();
        Tmp.text = parentLetter.Tmp.text;
        OriginalParentLetter = parentLetter;
    }

    public void MoveToAnswerContainer(AnswerContainer newParentAnswerContainer)
    {
        gameObject.SetActive(true);
        
        CurrentAnswerContainer = newParentAnswerContainer;
        CurrentAnswerContainer.SetVisualToFull();

        RectTransform.SetParent(newParentAnswerContainer.Rect);
        ResetTransformToZero();
    }

    public void MoveToParentLetter()
    {
        gameObject.SetActive(false);

        if (CurrentAnswerContainer != null)
        {
            CurrentAnswerContainer.SetVisualToDefault();
            CurrentAnswerContainer = null;
        }

        RectTransform.SetParent(OriginalParentLetter.Rect);
        ResetTransformToZero();
    }

    public void ResetTransformToZero()
    {
        // Reset the scale and rotation to match the parent
        RectTransform.localScale = Vector3.one;
        RectTransform.localRotation = Quaternion.identity;

        // Center the RectTransform
        RectTransform.anchoredPosition = Vector2.zero;
        RectTransform.offsetMin = Vector2.zero;
        RectTransform.offsetMax = Vector2.zero;
    }
}
