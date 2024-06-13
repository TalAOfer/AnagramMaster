using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private bool _inputEnabled;

    private readonly List<BankLetter> usedLetters = new();

    [SerializeField] private LetterBank letterBank;
    [SerializeField] private GuessManager guessManager;
    [SerializeField] private GuessHistoryManager guessHistoryManager;
    [SerializeField] private AnswerManager answerManager;
    [SerializeField] private ElementFader fader;

    [SerializeField] private LevelBank levelBank;

    private GameData data;

    public void Initialize(GameData data)
    {
        this.data = data;
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
        if (!_inputEnabled || usedLetters.Count == 0)
        {
            UseLetter(clickedLetter);
        }
    }

    public void OnLetterHovered(Component sender, object data)
    {
        BankLetter hoveredLetter = (BankLetter)sender;
        if (!_inputEnabled || usedLetters.Count == 0) return;

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

        letter.ToggleContainer(true);
        letter.ChangeColor(true);
        usedLetters.Add(letter);
        int index = usedLetters.Count - 1;
        guessManager.AddLetter(letter.GuessLetter, index);
    }

    public void RemoveLastUsedLetter()
    {
        if (usedLetters.Count == 0) return;

        BankLetter lastLetter = usedLetters[^1];
        lastLetter.ToggleContainer(false);
        lastLetter.ChangeColor(false);
        usedLetters.RemoveAt(usedLetters.Count - 1);
        lastLetter.ResetGuessLetterToBankLetterTransform();
    }

    private string GetGuess()
    {
        string guess = "";

        foreach (BankLetter letterChar in usedLetters)
        {
            guess += letterChar.Tmp.text;
        }

        return guess;
    }

    public IEnumerator OnPointerUp()
    {
        if (usedLetters.Count == 0) yield break;

        foreach (BankLetter letter in usedLetters)
        {
            letter.ToggleContainer(false);
            letter.ChangeColor(false);
        }

        _inputEnabled = false;

        bool lastWordInLevel = data.NextLetters.Count == 0;

        if (data.CurrentLetters.Length == usedLetters.Count)
        {
            if (IsFoundInDictionary(GetGuess()))
            {
                yield return OnCorrectGuess();

                if (lastWordInLevel)
                {
                    //Do wave
                    yield return new WaitForSeconds(0.25f);
                    yield return fader.FadeGameplayToWinning();

                }
            }

            else
            {
                yield return PlayMistakeAnimation();
            }
        }

        foreach (BankLetter letter in usedLetters)
        {
            letter.ResetGuessLetterToBankLetterTransform();
        }

        usedLetters.Clear();

        _inputEnabled = true;
    }

    public IEnumerator OnCorrectGuess()
    {
        yield return guessManager.CorrectGuessAnimation();
        yield return answerManager.OnNewAnswer(usedLetters);

        bool isFinished = data.UpdateLevelData(GetGuess());

        //guessHistoryManager.HandleNewGuessedNode();

        if (isFinished) yield break;

        letterBank.ActivateNextLetter();

        guessManager.ActivateNextContainer();
    }

    public IEnumerator PlayMistakeAnimation()
    {
        yield return guessManager.MistakeAnimation();
    }

    private bool IsFoundInDictionary(string answer)
    {
        string[] possibleAnswers = levelBank.Value[data.Index].PossibleAnswers;
        return possibleAnswers.Any(a => a.Equals(answer, StringComparison.OrdinalIgnoreCase));
    }

    #endregion
}