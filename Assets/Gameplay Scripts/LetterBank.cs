using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime;
using System;
using System.Reflection;

public class LetterBank : MonoBehaviour
{
    [SerializeField] private Image circleImage;
    [SerializeField] private RectTransform circleTransform;
    [SerializeField] private List<BankLetter> PremadeLetters;
    [SerializeField] private List<Image> PremadeContainers;
    [SerializeField] private LetterBankLineManager lineManager;
    [SerializeField] private LevelBank levelBank;

    private GameData data;

    [ShowInInspector]
    private readonly List<Image> _activeContainers = new();

    [ShowInInspector]
    private readonly List<BankLetter> _activeLetters = new();

    private string GetLetterStringByIndex(int index) => data.CurrentLetters[index].ToString();
    
    #region Initialization & Spawning


    public void Initialize(GameData data)
    {
        this.data = data;
        Color levelColor = levelBank.Value[data.Index].containerBG;

        lineManager.Initialize(data, levelColor);

        _activeLetters.Clear();
        _activeContainers.Clear();

        for (int i = 0; i < PremadeLetters.Count; i++)
        {
            BankLetter letter = PremadeLetters[i];
            letter.Initialize(i, data, lineManager, levelColor);

            bool withinRangeOfCurrentUse = i < data.CurrentLetters.Length;

            if (withinRangeOfCurrentUse)
            {
                Image container = ActivateContainerByIndex(i);
                ActivateLetterByIndex(i);
                letter.SetAndSaveParentContainer(container);
                letter.Rect.anchoredPosition = Vector2.zero;
            }

            else
            {
                Image container = PremadeContainers[i];
                container.gameObject.SetActive(false);
                letter.gameObject.SetActive(false);
            }
        }

        SnapDistributeContainers();
    }

    public Image ActivateContainerByIndex(int index)
    {
        Image container = PremadeContainers[index];
        container.gameObject.SetActive(true);
        _activeContainers.Add(container);
        return container;
    }

    public BankLetter ActivateLetterByIndex(int index)
    {
        BankLetter letter = PremadeLetters[index];
        letter.gameObject.SetActive(true);
        _activeLetters.Add(letter);
        return letter;
    }

    public void ActivateNextLetter()
    {
        int nextLetterIndex = data.CurrentLetters.Length - 1;
        BankLetter letter = ActivateLetterByIndex(nextLetterIndex);
        Color temp = letter.Tmp.color;
        temp.a = 0;
        letter.Tmp.color = temp;

        Image container = ActivateContainerByIndex(nextLetterIndex);
        DistributeContainersOverTime();
       
        letter.SetAndSaveParentContainer(container);
        letter.Rect.DOAnchorPos(Vector2.zero, 1);
        letter.Tmp.DOFade(1, 1);
    }


    private List<Vector2> GetContainersPositions()
    {
        int containerCount = _activeContainers.Count;
        float angleStep = 360f / containerCount;
        float radius = circleTransform.sizeDelta.x / 3;

        List<Vector2> positions = new();

        for (int i = 0; i < _activeContainers.Count; i++)
        {
            float angle = (i * angleStep - 90) * Mathf.Deg2Rad; // Start at the top (270 degrees)
            Vector3 position = new(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            positions.Add(position);
        }

        return positions;
    }
    public void SnapDistributeContainers()
    {
        List<Vector2> containersPositions = GetContainersPositions();

        for (int i = 0; i < _activeContainers.Count; i++)
        {
            _activeContainers[i].rectTransform.anchoredPosition = containersPositions[i];
        }
    }

    public void DistributeContainersOverTime()
    {
        List<Vector2> containersPositions = GetContainersPositions();

        for (int i = 0; i < _activeContainers.Count; i++)
        {
            bool isLast = i == _activeContainers.Count - 1;
            if (isLast)
            {
                _activeContainers[i].rectTransform.anchoredPosition = containersPositions[i];
            }
            else
            {
                _activeContainers[i].rectTransform.DOAnchorPos(containersPositions[i], 1);
            }
        }
    }


    [Button]
    public void SnapDistributeContainersManually()
    {
        _activeContainers.Clear();

        foreach (Image container in PremadeContainers)
        {
            if (container.isActiveAndEnabled)
            {
                _activeContainers.Add(container);
            }
        }

        List<Vector2> containersPositions = GetContainersPositions();

        for (int i = 0; i < _activeContainers.Count; i++)
        {
            _activeContainers[i].rectTransform.anchoredPosition = containersPositions[i];
        }
    }


    public BankLetter FindLetter(string letterChar)
    {
        foreach (BankLetter letter in _activeLetters)
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