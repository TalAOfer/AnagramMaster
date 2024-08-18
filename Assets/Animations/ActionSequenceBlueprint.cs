using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ActionSequenceBlueprint
{
    [HorizontalGroup("Delay", Width = 110), LabelWidth(65)]
    public float PreDelay = 0;
    [HorizontalGroup("Delay", Width = 110, MarginLeft = 20), LabelWidth(65)]
    public float PostDelay = 0;

    public List<ActionBlueprint> SequenceBlueprint;

    public string GetDescription()
    {
        string desc = string.Empty;
        bool isFirst = true;

        foreach (ActionBlueprint animation in SequenceBlueprint)
        {
            if (animation.SequencingType is SequencingType.Join)
            {
                if (!isFirst)
                {
                    desc += " and ";
                }

                desc += animation.GetDescription();
            }

            else
            {
                if (!isFirst)
                {
                    desc += ", and then, \n";
                }

                desc += animation.GetDescription();
            }

            isFirst = false;
        }

        return desc;
    }

    public Sequence GetSequence(TweenableElement element)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(PreDelay);

        bool first = true;

        foreach (ActionBlueprint blueprint in SequenceBlueprint)
        {
            Sequence animationSequence = blueprint.GetSequence(element);

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

        sequence.AppendInterval(PostDelay);

        return sequence;
    }

    public Sequence Play(TweenableElement element)
    {
        return GetSequence(element).Play();
    }

    public IEnumerator PlayAndWait(TweenableElement element)
    {
        yield return Play(element).WaitForCompletion();
    }
}
