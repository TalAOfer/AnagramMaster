using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private bool _inputEnabled;

    private readonly List<BankLetter> usedLetters = new();

    [SerializeField] private GameDataManager gameDataManager;
    [SerializeField] private LetterBank LetterBank;
    [SerializeField] private AnswerManager AnswerManager;
    [SerializeField] private GuessManager GuessManager;
    [SerializeField] private HintManager HintManager;
    [SerializeField] private GuessHistoryManager GuessHistoryManager;

    [SerializeField] private ElementController ElementController;
    [SerializeField] private WinningManager winningManager;
    [SerializeField] private SoloAnimalUI SoloAnimalUI;
    [SerializeField] private TextMeshProUGUI levelTextImage;
    [SerializeField] private SwipeHand swipeHand;
    [SerializeField] private float tutorialPreDelay = 3f;
    [SerializeField] private float tutorialAnimationIntervals = 2f;

    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private GameData Data => AssetProvider.Instance.Data.Value;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;
    private EventRegistry Events => AssetProvider.Instance.Events;

    public void Initialize()
    {
        usedLetters.Clear();
        _inputEnabled = true;
        levelTextImage.text = "Level " + (Data.OverallLevelIndex + 1).ToString();

        SoloAnimalUI.Initialize(BiomeBank.GetAnimal(Data.IndexHierarchy));
        LetterBank.Initialize();
        GuessHistoryManager.Initialize();
        GuessManager.Initialize();
        AnswerManager.Initialize();
        HintManager.Initialize();
        
        Events.Ad_LoadInterstitial.Raise();

        if (Data.OverallLevelIndex > 6)
        {
            Events.Ad_DisplayBanner.Raise();
        }
    }

    public void InitializeHandSwipe()
    {
        StartCoroutine(HandSwipeRoutine());
    }
    public IEnumerator HandSwipeRoutine()
    {
        bool isFirstLevel = Data.IndexHierarchy.Level == 0;
        if (isFirstLevel)
        {
            yield return Tools.GetWaitRealtime(tutorialPreDelay);

            if (!DidSwipe)
            {
                swipeHand.ActivateObject();
                swipeHand.FadeInText();


                while (!DidSwipe)
                {
                    yield return swipeHand.PlayHandSequence().WaitForCompletion();

                    if (DidSwipe) break;

                    yield return Tools.GetWaitRealtime(tutorialAnimationIntervals);
                }

                swipeHand.FadeOutEverything();
            }
        }
    }

    private bool DidSwipe => Data.Level.CorrectAnswers.Count > 0 || usedLetters.Count > 1;


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
        SoundManager.PlaySound("Input", letter.transform.position);

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
        SoundManager.PlaySound("RemoveInput", lastLetter.transform.position);
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

    #region Answering

    public IEnumerator OnPointerUp()
    {
        if (usedLetters.Count == 0) yield break;

        ResetUsedLetters();

        _inputEnabled = false;
        HintManager.SetHintInteractability(false);

        if (Data.Level.CurrentLetters.Length == usedLetters.Count)
        {
            string guess = GetGuess();
            if (IsFoundInDictionary(guess))
            {
                if (IsSameWordWithS(guess))
                {
                    yield return HandlePluralMistake();
                }
                else
                {
                    yield return HandleCorrectGuess(guess);
                }
            }
            else
            {
                yield return PlayMistakeAnimation();
            }
        }

        ResetLetterState();

        _inputEnabled = true;
        HintManager.SetHintInteractability(true);
    }

    private void ResetUsedLetters()
    {
        foreach (BankLetter letter in usedLetters)
        {
            letter.ToggleContainer(false);
            letter.ChangeColor(false);
        }
    }

    private IEnumerator HandlePluralMistake()
    {
        BankLetter firstLetter = usedLetters[0];
        Color guessColor = firstLetter.GuessLetter.Tmp.color;
        Color answerColor = firstLetter.GuessLetter.AnswerLetter.Tmp.color;

        ChangeLetterColors(AnimationData.pluralWrongAnswerColor);

        StartCoroutine(AnswerManager.PlayMistakeAnimation());
        yield return PlayMistakeAnimation();

        RestoreLetterColors(answerColor, guessColor);
    }

    private void ChangeLetterColors(Color pluralWrongAnswerColor)
    {
        for (int i = 0; i < usedLetters.Count; i++)
        {
            BankLetter letter = usedLetters[i];
            letter.GuessLetter.AnswerLetter.Tmp.color = pluralWrongAnswerColor;
            bool isLast = (i == usedLetters.Count - 1);
            if (!isLast) letter.GuessLetter.Tmp.color = pluralWrongAnswerColor;
        }
    }

    private void RestoreLetterColors(Color answerColor, Color guessColor)
    {
        for (int i = 0; i < usedLetters.Count; i++)
        {
            BankLetter letter = usedLetters[i];
            letter.GuessLetter.AnswerLetter.Tmp.color = answerColor;
            bool isLast = (i == usedLetters.Count - 1);
            if (!isLast) letter.GuessLetter.Tmp.color = guessColor;
        }
    }

    private IEnumerator HandleCorrectGuess(string guess)
    {
        Data.OnCorrectAnswer(guess);

        //Animate
        SoundManager.PlaySound("RightAnswer", transform.position);
        yield return GuessManager.CorrectGuessAnimation();
        yield return AnswerManager.OnNewAnswer(usedLetters);
        GuessHistoryManager.HandleNewAnsweredNode();
        //

        if (!Data.Level.DidFinish)
        {
            LetterBank.ActivateNextLetter();
            GuessManager.ActivateNextContainer();
            HintManager.OnNewWord();
        }

        else
        {
            GameData currentGameData = Data.Clone();
            winningManager.Initialize(currentGameData);
            gameDataManager.LoadNextLevel();
            yield return Tools.GetWaitRealtime(0.25f);
            yield return winningManager.WinningRoutine();
        }
    }

    [Button]
    public void SkipLevel()
    {
        string guess = BiomeBank.GetHintWord(Data);
        Data.OnCorrectAnswer(guess);

        AnswerManager.OnNewAnswerImmediate(usedLetters);
        GuessHistoryManager.HandleNewAnsweredNode();

        if (!Data.Level.DidFinish)
        {
            LetterBank.ActivateNextLetter();
            GuessManager.ActivateNextContainer();
            HintManager.OnNewWord();
        }

        else
        {
            GameData currentGameData = Data.Clone();
            winningManager.Initialize(currentGameData);
            gameDataManager.LoadNextLevel();
            StartCoroutine(winningManager.WinningRoutine());
        }
    }

    private void ResetLetterState()
    {
        foreach (BankLetter letter in usedLetters)
        {
            letter.ResetGuessLetterToBankLetterTransform();
        }

        usedLetters.Clear();
    }

    private bool IsSameWordWithS(string guess)
    {
        if (Data.Level.CorrectAnswers.Count <= 0) return false;

        string lastAnswer = Data.Level.CorrectAnswers[^1];
        if (lastAnswer.EndsWith("s")) return false;

        string lastAnswerWithS = lastAnswer + "s";
        if (guess.ToUpper() == lastAnswerWithS.ToUpper())
        {
            return true;
        }

        return false;
    }

    #endregion

    public IEnumerator PlayMistakeAnimation()
    {
        SoundManager.PlaySound("WrongAnswer", transform.position);
        yield return GuessManager.MistakeAnimation();
    }

    private bool IsFoundInDictionary(string answer)
    {
        string[] possibleAnswers = BiomeBank.GetLevel(Data.IndexHierarchy).PossibleAnswers;
        return possibleAnswers.Any(a => a.Equals(answer, StringComparison.OrdinalIgnoreCase));
    }

    #endregion
}