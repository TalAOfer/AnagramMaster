
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuButton : UIButton
{
    [SerializeField] private TweenableElementPointer Main_BG;
    [SerializeField] private ElementController elementController;
    private EventRegistry Events => AssetProvider.Instance.Events;
    [SerializeField] private GameEvent A_InitializeFindTheAnimal;

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

        Sequence sequence = DOTween.Sequence();

        sequence.Append(AnimationData.S_Fade_Out.GetSequenceChain());
        sequence.Join(AnimationData.G_BG_Fade_In.GetSequenceChain());

        if (Data.IndexHierarchy.Level == 0)
        {
            A_InitializeFindTheAnimal.Raise();
            sequence.Append(AnimationData.Animal_Fade_In.GetSequenceChain());
            sequence.Append(AnimationData.Animal_Fade_Out.GetSequenceChain());
        }

        sequence.Append(AnimationData.G_Fade_In.GetSequenceChain());

        sequence.Play();
    }
}
