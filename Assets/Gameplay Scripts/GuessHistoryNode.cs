using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessHistoryNode : MonoBehaviour
{
    [SerializeField] private Image image;
    public Image Image { get { return image; } }
    public bool Answered { get; private set; } = false;
    [SerializeField] private Sprite pendingSprite;
    [SerializeField] private Sprite answeredSprite;
    public string Answer { get; private set; } = "";

    public void SetToAnswered(string answer)
    {
        Answered = true;
        image.sprite = answeredSprite;
        Answer = answer;
    }
}
