using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private LevelBlueprint levelBlueprint;
    [SerializeField] private LetterBank LetterBank;
    [SerializeField] private GuessHistoryManager GuessHistoryManager;
    [SerializeField] private TextMeshProUGUI LevelIndexText;
    [SerializeField] private GameplayManager GameplayManager;
    [SerializeField] private AnswerManager AnswerManager;

    public void Awake()
    {
        Initialize();
    }

    [Button]
    public void Initialize()
    {
        Level level = new(0, levelBlueprint);
        LetterBank.Initialize(level);
        GuessHistoryManager.Initialize(level);
        LevelIndexText.text = "Level " + (level.Index + 1).ToString();
        GameplayManager.Initialize(level);
        AnswerManager.Initialize(level);
    }
}
