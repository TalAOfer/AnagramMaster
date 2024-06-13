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
    
    private Image container;
    private Color activeContainerColor;
    [SerializeField] private Color ActiveTextColor;
    [SerializeField] private Color DefaultTextColor;
    private LetterBankLineManager lineManager;
    public void Initialize(int index, GameData data, LetterBankLineManager lineManager, Color activeContainerColor)
    {
        this.lineManager = lineManager;
        this.activeContainerColor = activeContainerColor;

        ResetAllNestedLetters();
        ChangeColor(false);

        bool withinRangeOfUseForLevel = index < data.CurrentLetters.Length + data.NextLetters.Count;
        if (withinRangeOfUseForLevel)
        {
            int indexInList;
            string letterText;

            bool withinRangeOfCurrentUse = index < data.CurrentLetters.Length;
            if (withinRangeOfCurrentUse)
            {
                indexInList = index;
                letterText = data.CurrentLetters[indexInList].ToString().ToUpper();
            } 
            
            else
            {
                indexInList = index - data.CurrentLetters.Length;
                letterText = data.NextLetters[indexInList].ToString().ToUpper();
            }

            Tmp.text = letterText;
            GuessLetter.Initialize(this);
        } 
    }

    public void SetAndSaveParentContainer(Image container)
    {
        rect.SetParent(container.transform);
        this.container = container;
        container.color = activeContainerColor;
    }

    public void ChangeColor(bool toActive) 
    {
        tmp.color = toActive ? ActiveTextColor : DefaultTextColor;
    }

    public Image ToggleContainer(bool toggle)
    {
        container.enabled = toggle;
        if (toggle)
        {
            lineManager.AddPoint(container.rectTransform.position);
        }

        else
        {
            lineManager.RemovePoint();
        }

        return container;
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

