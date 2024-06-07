using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private LetterBank LetterBank;
    [SerializeField] private GuessHistoryManager GuessHistoryManager;
    [SerializeField] private TextMeshProUGUI LevelIndexText;
    [SerializeField] private GameplayManager GameplayManager;
    [SerializeField] private GuessManager GuessManager;
    [SerializeField] private AnswerManager AnswerManager;

    [Button]
    public void Initialize(LevelBlueprint levelBlueprint)
    {
        Level level = new(0, levelBlueprint);
        LetterBank.Initialize(level);
        GuessHistoryManager.Initialize(level);
        LevelIndexText.text = "Level " + (level.Index + 1).ToString();
        GameplayManager.Initialize(level);
        GuessManager.Initialize(level);
        AnswerManager.Initialize();
    }
}
