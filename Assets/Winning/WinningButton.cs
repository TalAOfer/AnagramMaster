using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningButton : UIButton
{
    private EventRegistry Events => AssetProvider.Instance.Events;
    [SerializeField] private TweenableElementPointer Secondary_BG;
    [SerializeField] private ElementController elementController;

    private bool giveFeedback;
    public override void Initialize(NextLevelData nextLevelData)
    {
        string text = "";
        this.nextLevelData = nextLevelData;
        giveFeedback = false;

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
                giveFeedback = true;
                break;
        }

        Tmp.text = text;
        button.enabled = true;
    }

    protected override void DoAction()
    {
        if (giveFeedback)
        {
            return;
        }

        Events.OnStartButtonPressed.Raise();
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(AnimationData.W_Fade_Out.GetSequenceChain());
        
        if (nextLevelData.NextLevelEvent is NextLevelEvent.NewArea or NextLevelEvent.NewBiome)
        {
            elementController.GetElement(Secondary_BG).GetComponent<Image>().sprite = BiomeBank.GetArea(Data.IndexHierarchy).Sprite;
            sequence.Append(AnimationData.G_BG_Switch.GetSequenceChain());    
            sequence.Append(AnimationData.Animal_Hidden_Fade_In.GetSequenceChain());
            sequence.Append(AnimationData.Animal_Hidden_Fade_Out.GetSequenceChain());
        }

        sequence.Append(AnimationData.G_Fade_In.GetSequenceChain());

        sequence.Play();
    }

}
