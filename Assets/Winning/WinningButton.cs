using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningButton : UIButton
{
    private EventRegistry Events => AssetProvider.Instance.Events;

    public override void Initialize(NextLevelData nextLevelData)
    {
        string text = "";
        this.nextLevelData = nextLevelData;

        switch (nextLevelData.NextLevelEvent)
        {
            case NextLevelEvent.None:
                text = "Level " + (Data.OverallLevelIndex + 2).ToString();
                break;
            case NextLevelEvent.NewArea:
                text = "Level " + (Data.OverallLevelIndex + 2).ToString();
                break;
            case NextLevelEvent.NewBiome:
                text = "To The " + (BiomeBank.Biomes[nextLevelData.IndexHierarchy.Biome].name.ToString()) + "!";
                break;
            case NextLevelEvent.FinishedGame:
                text = "Give Feedback";
                break;
        }

        Tmp.text = text;
        button.enabled = true;
    }

    protected override void DoAction()
    {
        Events.OnStartButtonPressed.Raise();

        switch (nextLevelData.NextLevelEvent)
        {
            case NextLevelEvent.None:
                elementController.WinningToGameplay();
                break;
            case NextLevelEvent.NewArea:
                elementController.FadeWinningNewAreaToGameplay();
                break;
            case NextLevelEvent.NewBiome:
                elementController.WinningToGameplay();
                break;
            case NextLevelEvent.FinishedGame:
                Application.OpenURL("https://forms.gle/pJEPRzfHPLv17fu5A");
                break;
        }
    }
}
