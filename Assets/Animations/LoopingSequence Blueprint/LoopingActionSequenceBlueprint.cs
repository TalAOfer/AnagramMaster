using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Looping Action", order = 2)]
public class LoopingActionSequenceBlueprint : ScriptableObject
{
    [HorizontalGroup("Delay", Width = 110), LabelWidth(65)]
    public float PreDelay = 0;
    [HorizontalGroup("Delay", Width = 110, MarginLeft = 20), LabelWidth(65)]
    public float PostDelay = 0;
    [HorizontalGroup("Delay", Width = 110, MarginLeft = 20), LabelWidth(65)]
    public float LoopInterval = 0.5f;

    public ActionSequenceBlueprintRef ActionSequence;

    public Sequence GetSequence(List<TweenableElement> elements)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(PreDelay);

        for (int i = 0; i < elements.Count; i++)
        {
            TweenableElement element = elements[i];
            Sequence animationSequence = ActionSequence.GetSequence(element);

            float startTime = i * LoopInterval;
            sequence.Insert(startTime, animationSequence);  
        }

            sequence.AppendInterval(PostDelay);

            return sequence;
        }
        public Sequence Play(List<TweenableElement> elements)
        {
            return GetSequence(elements).Play();
        }

        public IEnumerator PlayAndWait(List<TweenableElement> elements)
        {
            yield return Play(elements).WaitForCompletion();
        }

    }
