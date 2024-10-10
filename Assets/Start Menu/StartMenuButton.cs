
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
    [SerializeField] private Sprite NormalSprite, HardSprite, VeryHardSprite;

    public override void Initialize(NextLevelData nextLevelData)
    {
        string text;
        this.nextLevelData = nextLevelData;

        elementController.GetElement(Main_BG).GetComponent<Image>().sprite = BiomeBank.GetArea(Data.IndexHierarchy).Sprite;

        switch (BiomeBank.GetLevel(Data.IndexHierarchy).Difficulty)
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

        bool finishedGame = Data.Level.DidFinish && nextLevelData.NextLevelEvent is NextLevelEvent.FinishedGame;

        if (finishedGame)
        {
            text = "More Levels Soon!";
            button.interactable = false;
            button.image.sprite = NormalSprite;
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

        sequence.Append(AnimationData.Animal_Hidden_Fade_In.GetSequenceChain());
        sequence.Append(AnimationData.Animal_Hidden_Fade_Out.GetSequenceChain());

        sequence.Append(AnimationData.G_Fade_In.GetSequenceChain());

        sequence.Play();
    }
}
