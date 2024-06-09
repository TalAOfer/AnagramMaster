using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BankLetterState
{
    Selectable,
    First,
    UsedButNotFirst
}

public class BankLetter : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private TextMeshProUGUI tmp;
    public TextMeshProUGUI Tmp { get { return tmp; } private set { } }
    [SerializeField] private RectTransform rect;
    public RectTransform Rect { get { return rect; } private set { } }

    [SerializeField] private GameEvent LetterPointerDown;
    [SerializeField] private GameEvent LetterPointerEnter;
    [SerializeField] private GuessLetter guessLetter;
    public GuessLetter GuessLetter { get { return guessLetter; } private set { } } 

    public void Initialize(string letter)
    {
        Tmp.text = letter.ToUpper();
        GuessLetter.Initialize(this);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        LetterPointerDown.Raise(this, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LetterPointerEnter.Raise(this, this);
    }


    public void ResetGuessLetterToBankLetterTransform()
    {
        GuessLetter.MoveToParentLetter();
    }

    public void ResetAllNestedLetters()
    {
        GuessLetter.gameObject.SetActive(false);
        GuessLetter.MoveToParentLetter();
        GuessLetter.AnswerLetter.SetUsed(false);
        GuessLetter.AnswerLetter.ResetTransformToZero();
    }


}

