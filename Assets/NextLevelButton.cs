using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private ElementFader fader;
    [SerializeField] private bool fromWinning;
    [SerializeField] private TextMeshProUGUI Tmp;
    [SerializeField] private Tweener tweener;
    [SerializeField] private TweenBlueprint buttonClickAnim;
    private NextLevelData nextLevelData;
    private GameData Data => AssetProvider.Instance.Data.Value;

    public void Initialize(NextLevelData nextLevelData)
    {
        string text;
        this.nextLevelData = nextLevelData;

        if (fromWinning)
        {
            if (nextLevelData.NextLevelType is not NextLevelEvent.FinishedGame)
            {
                text = "Level " + (Data.OverallLevelIndex + 2).ToString();
            }

            else 
            {
                text = "Thank You!"; 
            }
        }

        else
        {
            bool finishedGame = nextLevelData.NextLevelType is NextLevelEvent.FinishedGame;

            if (finishedGame)
            {
                text = "More Levels Soon!";
                button.interactable = false;
            }

            else
            {
                text = "Level " + (Data.OverallLevelIndex + 1).ToString();
            }
        }


        Tmp.text = text;
        button.enabled = true;
    }

    public void OnClick()
    {
        button.enabled = false;
        SoundManager.PlaySound("ButtonClick", transform.position);
        StartCoroutine(ClickSequence());
    }

    public IEnumerator ClickSequence()
    {
        yield return tweener.TriggerTween(buttonClickAnim).WaitForCompletion();
        if (fromWinning)
        {
            switch (nextLevelData.NextLevelType)
            {
                case NextLevelEvent.None:
                    fader.WinningToGameplay();
                    break;
                case NextLevelEvent.NewArea:
                    fader.FadeWinningNewAreaToGameplay();
                    break;
                case NextLevelEvent.NewBiome:
                    fader.WinningToGameplay();
                    break;
                case NextLevelEvent.FinishedGame:
                    fader.WinningToStartMenu();
                    break;
            }
        }

        else
        {
            fader.FadeStartMenuToGameplay();
        }
    }
}

