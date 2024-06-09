using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuessManager : MonoBehaviour
{
    [SerializeField] private List<GuessContainer> PremadeGuessContainers;
    private readonly List<GuessContainer> _activeGuessContainers = new();

    public void Initialize(Level level)
    {
        _activeGuessContainers.Clear();

        for (int i = 0; i < PremadeGuessContainers.Count; i++)
        {
            GuessContainer currentContainer = PremadeGuessContainers[i];
            if (i < level.CurrentLetters.Length)
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

    public IEnumerator CorrectGuessAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var guessContainer in _activeGuessContainers)
        {
            sequence.Append(guessContainer.transform.DOPunchPosition(Vector3.up * 5, 0.15f));
        }

        sequence.AppendInterval(0.15f);

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator MistakeAnimation()
    {

        transform.DOShakePosition(0.5f, 5, 10, 0, false, true, ShakeRandomnessMode.Harmonic);

        yield return null;
    }


}
