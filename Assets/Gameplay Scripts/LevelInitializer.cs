using Sirenix.OdinInspector;
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
    public void ResetData()
    {
        GameData data = new();
        SaveSystem.Save(data);
    }

    public void Initialize(GameData data)
    {
        LetterBank.Initialize(data);
        GuessHistoryManager.Initialize(data);
        LevelIndexText.text = "Level " + (data.Index + 1).ToString();
        GameplayManager.Initialize(data);
        GuessManager.Initialize(data);
        AnswerManager.Initialize(data);
    }
}
