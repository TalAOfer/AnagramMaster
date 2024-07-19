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

    public Sequence Play(TweenableElement element)
    {
        return Blueprint.GetSequence(element).Play();
    }

    public IEnumerator PlayAndWait(TweenableElement element)
    {
        yield return Play(element).WaitForCompletion();
    }
}


[CreateAssetMenu(menuName ="Animations/Sequence", order = 2)]
public class ActionSequenceBlueprintAsset : ScriptableObject
{
    public ActionSequenceBlueprint Value;
}