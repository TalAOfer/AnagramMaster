using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Level")]
public class LevelBlueprint : ScriptableObject
{
    public string StartingLetters;
    public string[] NextLetters;
}
