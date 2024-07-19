using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessManager : MonoBehaviour
{
    [SerializeField] private List<GuessContainer> premadeGuessContainers;
    private readonly List<GuessContainer> _activeGuessContainers = new();
    public List<GuessContainer> ActiveGuessContainers { get { return _activeGuessContainers; } }

    [SerializeField] private TweenableElement element;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;
    private GameData Data => AssetProvider.Instance.Data.Value;

    public void Initialize()
    {
        _activeGuessContainers.Clear();

        Color areaColor = BiomeBank.GetArea(Data.IndexHierarchy).LetterContainerBGColor;

        for (int i = 0; i < premadeGuessContainers.Count; i++)
        {
            GuessContainer currentContainer = premadeGuessContainers[i];

            currentContainer.Initialize(areaColor);

            if (i < Data.Level.CurrentLetters.Length)
            {
                currentContainer.gameObject.SetActive(true);
                _activeGuessContainers.Add(currentContainer);
            }

            else
            {
                currentContainer.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateNextContainer()
    {
        GuessContainer nextGuessContainer = premadeGuessContainers[_activeGuessContainers.Count];
        nextGuessContainer.gameObject.SetActive(true);
        _activeGuessContainers.Add(nextGuessContainer);
    }

    public void AddLetter(GuessLetter letter, int index)
    {
        GuessContainer guessContainer = _activeGuessContainers[index];
        letter.MoveToGuessContainer(guessContainer);
    }

    [Button]
    public void TestCorrectGuessAnimation()
    {
        StartCoroutine(CorrectGuessAnimation());
    }

    public IEnumerator CorrectGuessAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var guessContainer in _activeGuessContainers)
        {
            sequence.AppendCallback(() => guessContainer.StartCorrectAnswerAnimation());
            sequence.AppendCallback(() => SoundManager.PlaySound("LetterBounce", guessContainer.transform.position));
            sequence.AppendInterval(AnimData.CorrectGuessAnimDelayBetweenLetters);
        }

        sequence.AppendInterval(AnimData.PostCorrectGuessAnimDelay);

        yield return sequence.WaitForCompletion();
    }

    [Button]
    public void TestMistakeAnimation()
    {
        StartCoroutine(MistakeAnimation());
    }

    public IEnumerator MistakeAnimation()
    {
        yield return AnimData.ProgressionFruitAnimation.PlayAndWait(element);
    }


}
