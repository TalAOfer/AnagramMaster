using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] private GameObject AnswerContainerNodePrefab;
    [SerializeField] private List<AnswerContainer> PremadeAnswerContainers;
    private readonly List<AnswerContainer> _activeAnswerContainers = new();

    public void Initialize(Level level)
    {
        for (int i = 0; i < PremadeAnswerContainers.Count; i++)
        {
            AnswerContainer currentContainer = PremadeAnswerContainers[i];
            if (i < level.CurrentLetters.Length)
            {
                currentContainer.gameObject.SetActive(true);
                _activeAnswerContainers.Add(currentContainer);
            } 
            
            else
            {
                currentContainer.gameObject.SetActive(false);
            }
        }
    }
    public void ActivateNextContainer()
    {
        AnswerContainer nextAnswerContainer = PremadeAnswerContainers[_activeAnswerContainers.Count];
        nextAnswerContainer.gameObject.SetActive(true);
        _activeAnswerContainers.Add(nextAnswerContainer);
    }

    public void AddLetter(AnswerLetter letter, int index)
    {
        AnswerContainer answerContainer = _activeAnswerContainers[index];
        letter.MoveToAnswerContainer(answerContainer);
    }

    public IEnumerator CorrectAnswerAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var answerContainer in _activeAnswerContainers)
        {
            sequence.Append(answerContainer.transform.DOPunchPosition(Vector3.up * 5, 0.15f));
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
