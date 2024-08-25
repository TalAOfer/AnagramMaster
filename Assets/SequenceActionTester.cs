using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceActionTester : MonoBehaviour
{
    [SerializeField] private SequenceChainBlueprint chain;
    private Sequence _currentSequence;

    [Button]
    public void Test()
    {
        FinishCurrent();

        _currentSequence = chain.Play();
    }

    [Button]
    public void FinishCurrent()
    {
        _currentSequence?.Complete();
    }
}
