using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GuessHistoryNode : MonoBehaviour
{
    [SerializeField] private Image image;
    public Image Image { get { return image; } }
    public bool Answered { get; private set; } = false;
    private Sprite pendingSprite;
    private Sprite answeredSprite;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;
    [SerializeField] private TweenableElement element;
    public string Answer { get; private set; } = "";

    public void Initialize(Biome biome)
    {
        pendingSprite = biome.EmptyCollectible;
        answeredSprite = biome.FullCollectible;
        Answered = false;

        image.sprite = pendingSprite;
    }

    [Button]
    public void TestAnimation()
    {
        AnimationData.ProgressionIconAnimation.Play(element);
    }

    public void SetToAnswered(string answer)
    {
        Answered = true;
        image.sprite = answeredSprite;
        AnimationData.ProgressionIconAnimation.Play(element);
        Answer = answer;
    }
}
