using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

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
        letter.gameObject.SetActive(true);
        letter.Initialize(letterStr);
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

    #endregion
}