using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum TweenType
{
    None,
    PunchScale,
    ShakeScale,
    PunchPosition,
    ShakePosition,
    PunchRotation,
    ShakeRotation
}

[CreateAssetMenu(menuName ="Tweener/Tween Blueprint")]
public class TweenBlueprint : ScriptableObject
{
    public TweenType type;
    
    public Ease ease;

    public float Duration = 0.25f;
    [HideIf("@ShowRotationVariables()")]
    public float Strength = 0.1f;
    [ShowIf("type", TweenType.ShakeRotation)]
    public Vector3 RotationStrength = Vector3.one;
    public int Vibrato = 10;
    [ShowIf("@ShowShakeVariables()")]
    public float Randomness = 25f;
    [ShowIf("@ShowPunchVariables()")]
    public float Elasticity = 1f;
    [ShowIf("@ShowPunchVariables()")]
    public Vector3 Punch = Vector3.up;

    private bool ShowRotationVariables()
    {
        return type is TweenType.PunchRotation or TweenType.ShakeRotation;
    }

    private bool ShowPunchVariables()
    {
        return type is TweenType.PunchScale or TweenType.PunchPosition or TweenType.PunchRotation;
    }

    private bool ShowShakeVariables()
    {
        return type is TweenType.ShakeScale or TweenType.ShakePosition or TweenType.ShakeRotation;
    }
}
