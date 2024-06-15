using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinOnEnable : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 0, 360);
    [SerializeField] private float duration = 1f;
    [SerializeField] private RotateMode rotateMode = RotateMode.FastBeyond360;

    private void OnEnable()
    {
        transform.DORotate(rotationAxis, duration, rotateMode)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }
}
