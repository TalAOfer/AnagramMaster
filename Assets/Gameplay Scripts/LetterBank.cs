using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime;

public class LetterBank : MonoBehaviour
{
    [SerializeField] private Image circleImage;
    [SerializeField] private RectTransform circleTransform;
    [SerializeField] private List<BankLetter> PremadeLetters;
    public List<BankLetter> ActiveLetters { get; private set; } = new();
    private Level level;

    #region Initialization & Spawning

    public void Initialize(Level level)
    {
        this.level = level;
        ActiveLetters.Clear();

        for (int i = 0; i < PremadeLetters.Count; i++)
        {
            BankLetter letter = PremadeLetters[i];
            letter.ResetAllNestedLetters();

            if (i < level.CurrentLetters.Length)
            {
                EnableNextLetter(i);
            }

            else
            {
                PremadeLetters[i].gameObject.SetActive(false);
            }
        }

        DistributeLetters();
    }

    public BankLetter EnableNextLetter(int index)
    {
        string letterStr = level.CurrentLetters[index].ToString();
        BankLetter letter = PremadeLetters[index];
        letter.Initialize(letterStr);
        letter.gameObject.SetActive(true);
        ActiveLetters.Add(letter);
        return letter;
    }

    public void DistributeLetters()
    {
        int letterCount = ActiveLetters.Count;
        float angleStep = 360f / letterCount;
        float radius = circleTransform.sizeDelta.x / 3;

        for (int i = 0; i < letterCount; i++)
        {
            float angle = (i * angleStep - 90) * Mathf.Deg2Rad; // Start at the top (270 degrees)
            Vector3 position = new(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            ActiveLetters[i].Rect.anchoredPosition = position; // Use anchoredPosition to correctly position within UI
        }
    }

    public BankLetter FindLetter(string letterChar)
    {
        foreach(BankLetter letter in ActiveLetters)
        {
            if (letter.Tmp.text == letterChar && 
                !letter.GuessLetter.AnswerLetter.IsUsed)
            {
                return letter;
            }
        }

        Debug.Log("No letter found");
        return null;
    }

    #endregion
}