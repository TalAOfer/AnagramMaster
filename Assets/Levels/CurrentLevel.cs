using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Current Level")]
public class CurrentLevel : ScriptableObject
{
    public bool IsInitialized;
    public Level Value;
}
