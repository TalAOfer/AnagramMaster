using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuessManager : MonoBehaviour
{
    [SerializeField] private List<GuessContainer> PremadeGuessContainers;
    private readonly List<GuessContainer> _activeGuessContainers = new();
    
    [SerializeField] private Tweener tweener;
    private LevelBank LevelBank => AssetLocator.Instance.LevelBank;
    private AnimationData AnimData => AssetLocator.Instance.AnimationData;

    public void Initialize(GameData data)
    {
        _activeGuessContainers.Clear();

        for (int i = 0; i < PremadeGuessContainers.Count; i++)
        {
            GuessContainer currentContainer = PremadeGuessContainers[i];
            currentContainer.Initialize(LevelBank.Value[data.Index].containerBG);

            if (i < data.CurrentLetters.Length)
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
        GuessContainer nextGuessContainer = PremadeGuessContainers[_activeGuessContainers.Count];
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
            sequence.AppendCallback(()=> guessContainer.StartCorrectAnswerAnimation());
            sequence.AppendInterval(AnimData.correctGuessAnimaDelayBetweenLetters);
        }

        sequence.AppendInterval(AnimData.postCorrectGuessAnimDelay);

        yield return sequence.WaitForCompletion();
    }

    [Button]
    public void TestMistakeAnimation()
    {
        StartCoroutine(MistakeAnimation());
    }

    public IEnumerator MistakeAnimation()
    {

        tweener.TriggerTween(AnimData.guessMistakeAnimBlueprint);

        yield return null;
    }


}
