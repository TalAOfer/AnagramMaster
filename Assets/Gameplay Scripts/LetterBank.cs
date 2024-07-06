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
    [SerializeField] private List<BankLetterContainer> PremadeContainers;
    [SerializeField] private LetterBankLineManager lineManager;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;

    private GameData Data => AssetProvider.Instance.Data.Value;

    [ShowInInspector]
    public List<BankLetterContainer> ActiveContainers { get; private set; } = new();

    [ShowInInspector]
    private readonly List<BankLetter> _activeLetters = new();
    
    #region Initialization & Spawning


    public void Initialize()
    {
        Color levelColor = BiomeBank.GetArea(Data.IndexHierarchy).LetterContainerBGColor;

        lineManager.Initialize(levelColor);

        _activeLetters.Clear();
        ActiveContainers.Clear();

        for (int i = 0; i < PremadeLetters.Count; i++)
        {
            BankLetter letter = PremadeLetters[i];
            letter.Initialize(i, lineManager, levelColor);

            bool withinRangeOfCurrentUse = i < Data.Level.CurrentLetters.Length;

            if (withinRangeOfCurrentUse)
            {
                BankLetterContainer container = ActivateContainerByIndex(i);
                ActivateLetterByIndex(i);
                letter.SetAndSaveParentContainer(container);
                letter.Rect.anchoredPosition = Vector2.zero;
            }

            else
            {
                BankLetterContainer container = PremadeContainers[i];
                container.gameObject.SetActive(false);
                letter.gameObject.SetActive(false);
            }
        }

        SnapDistributeContainers();
    }

    public BankLetterContainer ActivateContainerByIndex(int index)
    {
        BankLetterContainer container = PremadeContainers[index];
        container.gameObject.SetActive(true);
        ActiveContainers.Add(container);
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
        int nextLetterIndex = Data.Level.CurrentLetters.Length - 1;
        BankLetter letter = ActivateLetterByIndex(nextLetterIndex);
        Color temp = letter.Tmp.color;
        temp.a = 0;
        letter.Tmp.color = temp;

        BankLetterContainer container = ActivateContainerByIndex(nextLetterIndex);
        DistributeContainersOverTime();
       
        letter.SetAndSaveParentContainer(container);
        letter.Rect.DOAnchorPos(Vector2.zero, 1);
        letter.Tmp.DOFade(1, 1);
    }


    private List<Vector2> GetContainersPositions()
    {
        int containerCount = ActiveContainers.Count;
        float angleStep = 360f / containerCount;
        float radius = circleTransform.sizeDelta.x / 3;

        List<Vector2> positions = new();

        for (int i = 0; i < ActiveContainers.Count; i++)
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

        for (int i = 0; i < ActiveContainers.Count; i++)
        {
            ActiveContainers[i].Rect.anchoredPosition = containersPositions[i];
        }
    }

    public void DistributeContainersOverTime()
    {
        List<Vector2> containersPositions = GetContainersPositions();
        SoundManager.PlaySound("BankReorganize", transform.position);

        for (int i = 0; i < ActiveContainers.Count; i++)
        {
            bool isLast = i == ActiveContainers.Count - 1;
            if (isLast)
            {
                ActiveContainers[i].Rect.anchoredPosition = containersPositions[i];
            }
            else
            {
                ActiveContainers[i].Rect.DOAnchorPos(containersPositions[i], 1);
            }
        }
    }


    [Button]
    public void SnapDistributeContainersManually()
    {
        ActiveContainers.Clear();

        foreach (BankLetterContainer container in PremadeContainers)
        {
            if (container.isActiveAndEnabled)
            {
                ActiveContainers.Add(container);
            }
        }

        List<Vector2> containersPositions = GetContainersPositions();

        for (int i = 0; i < ActiveContainers.Count; i++)
        {
            ActiveContainers[i].Rect.anchoredPosition = containersPositions[i];
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