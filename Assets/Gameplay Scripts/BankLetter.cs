using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GuessLetter GuessLetterInstance { get; private set; }

    public BankLetterState State { get; private set; }

    public void Initialize(string letter)
    {
        Tmp.text = letter.ToUpper();

        bool includeInactive = true;
        GuessLetterInstance = GetComponentInChildren<GuessLetter>(includeInactive);
        GuessLetterInstance.Initialize(this);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        LetterPointerDown.Raise(this, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LetterPointerEnter.Raise(this, this);
    }

    public void SetState(BankLetterState state)
    {
        this.State = state;
    }

    public void ResetGuessLetterToBankLetterTransform()
    {
        GuessLetterInstance.MoveToParentLetter();
    }


}

