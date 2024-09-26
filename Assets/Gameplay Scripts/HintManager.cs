using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] private GuessManager guessManager;
    [SerializeField] private Button hintButtonButton;
    [SerializeField] private TextMeshProUGUI hintAmountTMP;
    private string currentHintWord;
    List<GuessContainer> ActiveGuessContainers => guessManager.ActiveGuessContainers;
    private GameData Data => AssetProvider.Instance.Data.Value;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private EventRegistry Events => AssetProvider.Instance.Events;

    public void Initialize()
    {
        UpdateHintText();
        OnNewWord();
    }

    public void UpdateHintText()
    {
        hintAmountTMP.text = Data.HintAmount.ToString();
    }

    public void SetHintInteractability(bool enable)
    {
        hintButtonButton.interactable = enable;
    }

    public void OnNewWord()
    {
        currentHintWord = BiomeBank.GetHintWord(Data);
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
            UpdateHintText();
        }
    }
    public bool TryUseHint()
    {
        if (Data.HintAmount <= 0)
        {
            Events.Ad_ShowRewarded.Raise();
            return false;
        }

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

    public void Get2Hints()
    {
        ChangeHintAmount(2);
    }

    public void ChangeHintAmount(int amount)
    {
        Data.ChangeHintAmount(amount);
        UpdateHintText();
    }
}
