using UnityEngine;

public class AnimalAlbumCardContainer : MonoBehaviour
{
    private SoloAnimalUI _childUI;
    public SoloAnimalUI ChildUI
    {
        get
        {
            if (_childUI == null)
            {
                _childUI = GetComponentInChildren<SoloAnimalUI>();
            }
            return _childUI;
        }
    }
}
