using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[Serializable]
public class ActionSequenceBlueprintRef
{
    public bool UseAsset;

    [ShowIf("UseAsset")]
    public ActionSequenceBlueprintAsset Asset;
    [HideIf("UseAsset")]
    public ActionSequenceBlueprint Value;
    public ActionSequenceBlueprint Blueprint => UseAsset ? Asset.Value : Value;

    public Sequence GetSequence(TweenableElement element)
    {
        return Blueprint.GetSequence(element);
    }

    public Sequence Play(TweenableElement element)
    {
        return Blueprint.Play(element);
    }

    public IEnumerator PlayAndWait(TweenableElement element)
    {
        yield return Blueprint.PlayAndWait(element);
    }
}


[CreateAssetMenu(menuName ="Animations/Sequence", order = 2)]
public class ActionSequenceBlueprintAsset : ScriptableObject
{
    public ActionSequenceBlueprint Value;
}