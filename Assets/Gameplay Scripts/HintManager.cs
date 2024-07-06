using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    [SerializeField] private GuessManager guessManager;
    private GameData Data => AssetProvider.Instance.Data.Value;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    [SerializeField] private HintButton hintButton;
    private string currentHintWord;
    List<GuessContainer> ActiveGuessContainers => guessManager.ActiveGuessContainers;

    public void Initialize()
    {
        hintButton.UpdateHintText();
        OnNewWord();
        ChangeHintAmount(3);
    }

    public void OnNewWord()
    {
        currentHintWord = BiomeBank.GetLevel(Data.IndexHierarchy).GetHintWord(Data.Level.CurrentLetters.Length);
        for (int i = 0; i < ActiveGuessContainers.Count; i++)
        {
            GuessContainer container = ActiveGuessContainers[i];
            container.ToggleHint(false);
            container.SetHintLetter(currentHintWord[i].ToString());
        }
    }

    public void UseHint()
    {
        if (TryUseHint())
        {
            hintButton.UpdateHintText();
        }
    }
    public bool TryUseHint()
    {
        if (Data.HintAmount <= 0) return false;

        List<GuessContainer> containersAvailableForHint = new();
        foreach (var container in ActiveGuessContainers)
        {
            if (!container.HintApplied)
            {
                containersAvailableForHint.Add(container);
            }
        }

        if (containersAvailableForHint.Count <= 0) return false;

        int rand = Random.Range(0, containersAvailableForHint.Count);
        GuessContainer chosenContainer = containersAvailableForHint[rand];
        chosenContainer.ToggleHint(true);
        SoundManager.PlaySound("HintApplied", chosenContainer.transform.position);
        chosenContainer.PlayHintAnimation();
        ChangeHintAmount(-1);
        return true;
    }

    public void ChangeHintAmount(int amount)
    {
        Data.ChangeHintAmount(amount);
        hintButton.UpdateHintText();
    }
}
