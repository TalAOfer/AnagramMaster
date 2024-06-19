using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private bool _inputEnabled;

    private readonly List<BankLetter> usedLetters = new();

    [SerializeField] private LetterBank LetterBank;
    [SerializeField] private GuessManager GuessManager;
    [SerializeField] private GuessHistoryManager GuessHistoryManager;
    [SerializeField] private AnswerManager AnswerManager;

    [SerializeField] private ElementFader fader;
    [SerializeField] private WinningManager winningManager;
    [SerializeField] private TextMeshProUGUI levelTextImage;

    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private GameData Data => AssetProvider.Instance.Data.Value;

    public void Initialize()
    {
        usedLetters.Clear();
        _inputEnabled = true;
        levelTextImage.text = "Level " + (Data.OverallLevelIndex + 1).ToString();

        LetterBank.Initialize();
        GuessHistoryManager.Initialize();
        GuessManager.Initialize();
        AnswerManager.Initialize();
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
        GuessManager.AddLetter(letter.GuessLetter, index);
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

        bool lastWordInLevel = Data.NextLetters.Count == 0;

        if (Data.CurrentLetters.Length == usedLetters.Count)
        {
            if (IsFoundInDictionary(GetGuess()))
            {
                yield return OnCorrectGuess();

                if (lastWordInLevel)
                {
                    //Do wave
                    yield return new WaitForSeconds(0.25f);
                    winningManager.Initialize();
                    yield return winningManager.WinningRoutine();
                    yield break;
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
        Data.UpdateLevelData(GetGuess());

        yield return GuessManager.CorrectGuessAnimation();
        yield return AnswerManager.OnNewAnswer(usedLetters);

        GuessHistoryManager.HandleNewAnsweredNode();

        if (Data.DidFinish) yield break;

        LetterBank.ActivateNextLetter();

        GuessManager.ActivateNextContainer();
    }

    public IEnumerator PlayMistakeAnimation()
    {
        yield return GuessManager.MistakeAnimation();
    }

    private bool IsFoundInDictionary(string answer)
    {
        string[] possibleAnswers = BiomeBank.GetLevel(Data.IndexHierarchy).PossibleAnswers;
        return possibleAnswers.Any(a => a.Equals(answer, StringComparison.OrdinalIgnoreCase));
    }

    #endregion
}