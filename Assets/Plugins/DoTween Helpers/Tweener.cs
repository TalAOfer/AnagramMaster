using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    // Original properties to reset before applying a new effect
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Tween activeTween;
    [SerializeField] private bool test;
    [SerializeField] private bool resetScale = true;
    [SerializeField] private bool resetPosition = true;
    [SerializeField] private bool resetRotation = true;
    [ShowIf("test")][SerializeField] private TweenBlueprint testBlueprint;
    private void Start()
    {
        SaveOriginalTransform();
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
        ResetToOriginalTransform();
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }

    [ShowIf("test")]
    [Button]
    public void TestTween()
    {
        TriggerTween(testBlueprint);
    }

    private void SaveOriginalTransform()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    private void ResetToOriginalTransform()
    {
        if (resetScale)
            transform.localScale = originalScale;
        if (resetPosition)
            transform.localPosition = originalPosition;
        if (resetRotation) 
            transform.localRotation = originalRotation;
    }

    public Tween TriggerTween(TweenBlueprint blueprint)
    {
        Tween tween = null;

        if (blueprint == null)
        {
            Debug.LogError("No blueprint was sent to tweener");
            return null;
        }

        if (activeTween != null && !activeTween.IsComplete())
        {
            activeTween.Complete(); // This will jump to the end of the tween immediately
            ResetToOriginalTransform();
        }

        SaveOriginalTransform();

        switch (blueprint.type)
        {
            case TweenType.None:
                break;
            case TweenType.PunchScale:
                tween = TriggerPunchScale(blueprint);
                break;
            case TweenType.ShakeScale:
                tween = TriggerShakeScale(blueprint);
                break;
            case TweenType.PunchPosition:
                tween = TriggerPunchPosition(blueprint);
                break;
            case TweenType.ShakePosition:
                tween = TriggerShakePosition(blueprint);
                break;
            case TweenType.PunchRotation:
                tween = TriggerPunchRotation(blueprint);
                break;
            case TweenType.ShakeRotation:
                tween = TriggerShakeRotation(blueprint);
                break;
        }

        activeTween = tween;

        activeTween?.OnComplete(() =>
            {
                ResetToOriginalTransform();
                activeTween = null; // Clear the active tween since it's completed
            });

        return tween;
    }

    // Method to trigger a jiggle effect
    private Tween TriggerPunchPosition(TweenBlueprint blueprint)
    {
        return transform.DOPunchPosition(blueprint.Punch, blueprint.Duration, blueprint.Vibrato, blueprint.Elasticity)
            .SetUpdate(true)
            .SetEase(blueprint.ease);
    }
    private Tween TriggerShakePosition(TweenBlueprint blueprint)
    {
        return transform.DOShakePosition(blueprint.Duration, blueprint.Strength, blueprint.Vibrato, blueprint.Randomness)
            .SetUpdate(true)
            .SetEase(blueprint.ease);
    }

    private Tween TriggerPunchScale(TweenBlueprint blueprint)
    {
        return transform.DOPunchScale(blueprint.Punch, blueprint.Duration, blueprint.Vibrato, blueprint.Elasticity)
            .SetUpdate(true)
            .SetEase(blueprint.ease);
    }

    private Tween TriggerShakeScale(TweenBlueprint blueprint)
    {
        return transform.DOShakeScale(blueprint.Duration, blueprint.Strength, blueprint.Vibrato, blueprint.Randomness)
            .SetUpdate(true)
            .SetEase(blueprint.ease);
    }

    private Tween TriggerPunchRotation(TweenBlueprint blueprint)
    {
        return transform.DOPunchRotation(blueprint.Punch, blueprint.Duration, blueprint.Vibrato, blueprint.Elasticity)
            .SetUpdate(true)
            .SetEase(blueprint.ease);
    }

    private Tween TriggerShakeRotation(TweenBlueprint blueprint)
    {
        return transform.DOShakeRotation(blueprint.Duration, blueprint.RotationStrength, blueprint.Vibrato, blueprint.Randomness)
            .SetUpdate(true)
            .SetEase(blueprint.ease);
    }
}
