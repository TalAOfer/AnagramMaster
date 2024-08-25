
using System.Collections;
using UnityEngine;

public class BlackOverlay : MonoBehaviour
{
    private bool _didTap;
    public void OnBlackScreenTap() => _didTap = true;

    public IEnumerator AwaitBlackScreenInput()
    {
        _didTap = false;

        while (!_didTap)
        {
            yield return null;
        }
    }
}
