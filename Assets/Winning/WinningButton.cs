using DG.Tweening;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningButton : UIButton
{
    private EventRegistry Events => AssetProvider.Instance.Events;
    [SerializeField] private TweenableElementPointer Secondary_BG;
    [SerializeField] private ElementController elementController;
    [SerializeField] private Sprite NormalSprite, HardSprite, VeryHardSprite;

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

        switch (nextLevelData.LevelDifficulty)
        {
            case LevelDifficulty.Normal:
                button.image.sprite = NormalSprite;
                break;
            case LevelDifficulty.Hard:
                button.image.sprite = HardSprite;
                break;
            case LevelDifficulty.VeryHard:
                button.image.sprite = VeryHardSprite;
                break;
        }

        Tmp.text = text;
        button.enabled = true;
    }

    protected override void DoAction()
    {
        if (giveFeedback)
        {
            Application.OpenURL("https://forms.gle/68NxvQ5LkU6F2eM6A");
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
