using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Sequence Chain", order = 3)]
public class SequenceChainBlueprint : ScriptableObject
{
    [HorizontalGroup("Delay", Width = 110), LabelWidth(65)]
    public float PreDelay = 0;
    [HorizontalGroup("Delay", Width = 110, MarginLeft = 20), LabelWidth(65)]
    public float PostDelay = 0;
    public List<SequenceChainPieceBlueprint> Value;
    public Sequence GetSequenceChain()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(PreDelay);

        bool first = true;

        foreach (SequenceChainPieceBlueprint blueprint in Value)
        {
            Sequence animationSequence = blueprint.AnimationSequenceRef.Blueprint.GetSequence(GetElement(blueprint.ElementPointer));

            if (first || blueprint.SequencingType is SequencingType.Append)
            {
                sequence.Append(animationSequence);
            }

            else
            {
                sequence.Join(animationSequence);
            }

            first = false;
        }

        return sequence;
    }

    public Sequence Play()
    {
        return GetSequenceChain().Play();
    }

    public IEnumerator PlayAndWait()
    {
        yield return Play().WaitForCompletion();
    }

    public TweenableElement GetElement(TweenableElementPointer pointer)
    {
        return ElementController.Instance.GetElement(pointer);
    }
}

[Serializable]
public class SequenceChainPieceBlueprint
{
    [HorizontalGroup("Base"), HideLabel]
    public TweenableElementPointer ElementPointer;
    [HorizontalGroup("Base", 125)]
    [ColoredField("GetSequencingTypeColor"), HideLabel]
    public SequencingType SequencingType = SequencingType.Join;

    [BoxGroup, LabelText("Animations")]
    public ActionSequenceBlueprintRef AnimationSequenceRef;
    private Color GetSequencingTypeColor() => SequencingType == SequencingType.Append ? Color.red : Color.green;
}
