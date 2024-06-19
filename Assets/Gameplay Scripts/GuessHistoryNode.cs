using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessHistoryNode : MonoBehaviour
{
    [SerializeField] private Image image;
    public Image Image { get { return image; } }
    public bool Answered { get; private set; } = false;
    private Sprite pendingSprite;
    private Sprite answeredSprite;
    [SerializeField] private TweenBlueprint correctAnswerAnimation;
    [SerializeField] private Tweener tweener;
    public string Answer { get; private set; } = "";

    public void Initialize(Biome biome)
    {
        pendingSprite = biome.EmptyCollectible;
        answeredSprite = biome.FullCollectible;
        
        image.sprite = pendingSprite;
    }

    public void SetToAnswered(string answer)
    {
        Answered = true;
        image.sprite = answeredSprite;
        tweener.TriggerTween(correctAnswerAnimation);
        Answer = answer;
    }
}
