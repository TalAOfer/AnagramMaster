using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private NextLevelButton StartMenuButton;

    public void Initialize()
    {
        StartMenuButton.Initialize();
    }
}
