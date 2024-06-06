using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GuessLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    public TextMeshProUGUI Tmp { get { return tmp; } private set { } }
    [SerializeField] private RectTransform rect;
    public RectTransform Rect { get { return rect; } private set { } }
    [SerializeField] private AnswerLetter answerLetter;
    public AnswerLetter AnswerLetter { get { return answerLetter; } private set { } }

    public BankLetter OriginalParentLetter {  get; private set; }
    public GuessContainer CurrentGuessContainer { get; private set; }
    
    public void Initialize(BankLetter parentLetter)
    {
        tmp.text = parentLetter.Tmp.text;
        answerLetter.Initialize(this);
        OriginalParentLetter = parentLetter;
    }

    public void MoveToGuessContainer(GuessContainer newParentGuessContainer)
    {
        gameObject.SetActive(true);
        
        CurrentGuessContainer = newParentGuessContainer;
        CurrentGuessContainer.SetVisualToFull();

        rect.SetParent(newParentGuessContainer.Rect);
        ResetTransformToZero();
        if (!answerLetter.IsUsed) answerLetter.ResetTransformToZero();
    }

    public void MoveToParentLetter()
    {
        gameObject.SetActive(false);

        if (CurrentGuessContainer != null)
        {
            CurrentGuessContainer.SetVisualToDefault();
            CurrentGuessContainer = null;
        }

        rect.SetParent(OriginalParentLetter.Rect);
        ResetTransformToZero();
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
