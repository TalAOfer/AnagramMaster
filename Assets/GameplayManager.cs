using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private LetterBank letterBank;
    private bool _inputEnabled;

    private readonly List<BankLetter> usedLetters = new();

    [SerializeField] private AnswerManager answerManager;
    [SerializeField] private GuessHistoryManager guessHistoryManager;

    private Level level;

    public void Initialize(Level level)
    {
        this.level = level;
        _inputEnabled = true;
    }

    #region Gameplay

    private void Update()
    {
        // Check for mouse pointer up
        if (Input.GetMouseButtonUp(0) && _inputEnabled)
        {
            StartCoroutine(OnPointerUp());
        }

        // Check for touch pointer up
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && _inputEnabled)
            {
                StartCoroutine(OnPointerUp());
            }
        }
    }

    public void OnLetterPointerDown(Component sender, object data)
    {
        BankLetter clickedLetter = (BankLetter)sender;
        if (usedLetters.Count == 0)
        {
            UseLetter(clickedLetter);
        }
    }

    public void OnLetterHovered(Component sender, object data)
    {
        BankLetter hoveredLetter = (BankLetter)sender;
        if (usedLetters.Count == 0) return;

        if (usedLetters.Count >= 2 && hoveredLetter == usedLetters[^2])
        {
            RemoveLastUsedLetter();
        }
        else
        {
            UseLetter(hoveredLetter);
        }
    }

    public void UseLetter(BankLetter letter)
    {
        if (usedLetters.Contains(letter)) return; // Prevent re-adding the same letter

        if (usedLetters.Count == 0)
        {
            letter.SetState(BankLetterState.First);
        }
        else
        {
            letter.SetState(BankLetterState.UsedButNotFirst);
        }

        usedLetters.Add(letter);
        int index = usedLetters.Count - 1;
        answerManager.AddLetter(letter.AnswerLetterInstance, index);
    }

    public void RemoveLastUsedLetter()
    {
        if (usedLetters.Count == 0) return;

        BankLetter lastLetter = usedLetters[^1];
        usedLetters.RemoveAt(usedLetters.Count - 1);
        lastLetter.ResetAnswerLetterToBankLetterTransform();
    }

    private string GetAnswer()
    {
        string answer = "";

        foreach (BankLetter letterChar in usedLetters)
        {
            answer += letterChar.Tmp.text;
        }

        return answer;
    }

    public IEnumerator OnPointerUp()
    {
        if (usedLetters.Count == 0) yield break;

        _inputEnabled = false;

        if (letterBank.ActiveLetters.Count == usedLetters.Count && DictionaryLoader.Instance.IsWordValid(GetAnswer()))
        {
            yield return OnCorrectAnswer();
        }
        else
        {
            yield return OnMistake();
        }

        foreach (BankLetter letter in usedLetters)
        {
            letter.ResetAnswerLetterToBankLetterTransform();
        }

        usedLetters.Clear();

        _inputEnabled = true;
    }

    public IEnumerator OnCorrectAnswer()
    {
        yield return answerManager.CorrectAnswerAnimation();

        bool isFinished = level.OnCorrectAnswer(GetAnswer());

        guessHistoryManager.HandleNewAnsweredNode();

        if (isFinished)
        {
            Debug.Log("YOU WON!");
            yield break;
        }

        letterBank.EnableNextLetter(level.CurrentLetters.Length - 1);
        letterBank.DistributeLetters();
        answerManager.ActivateNextContainer();
    }

    public IEnumerator OnMistake()
    {
        yield return answerManager.MistakeAnimation();
        Debug.Log("Mistake.");
    }

    #endregion
}