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

    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;
    private GameData Data => AssetProvider.Instance.Data.Value;


    public void Initialize()
    {
        _activeGuessContainers.Clear();

        for (int i = 0; i < PremadeGuessContainers.Count; i++)
        {
            GuessContainer currentContainer = PremadeGuessContainers[i];
            currentContainer.Initialize(BiomeBank.GetArea(Data.IndexHierarchy).LetterContainerBGColor);

            if (i < Data.CurrentLetters.Length)
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
            sequence.AppendCallback(()=> SoundManager.PlaySound("LetterBounce", guessContainer.transform.position));
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
        yield return tweener.TriggerTween(AnimData.guessMistakeAnimBlueprint).WaitForCompletion();
    }


}
