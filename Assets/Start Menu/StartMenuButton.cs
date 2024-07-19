
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuButton : UIButton
{
    [SerializeField] private TweenableElementPointer Main_BG;
    [SerializeField] private ElementController elementController;
    private EventRegistry Events => AssetProvider.Instance.Events;
    public override void Initialize(NextLevelData nextLevelData)
    {
        string text;
        this.nextLevelData = nextLevelData;

        elementController.GetElement(Main_BG).GetComponent<Image>().sprite = BiomeBank.GetArea(Data.IndexHierarchy).Sprite;

        bool finishedGame = nextLevelData.NextLevelEvent is NextLevelEvent.FinishedGame;

        if (finishedGame)
        {
            text = "More Levels Soon!";
            button.interactable = false;
        }

        else
        {
            text = "Level " + (Data.OverallLevelIndex + 1).ToString();
        }

        Tmp.text = text;
        button.enabled = true;
    }

    protected override void DoAction()
    {
        Events.OnStartButtonPressed.Raise();

        AnimationData.S_Fade_Out.Play();
        AnimationData.G_Fade_In.Play();
    }
}
