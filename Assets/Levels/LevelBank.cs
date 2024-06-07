using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Level Bank")]
public class LevelBank : ScriptableObject
{
    public List<LevelBlueprint> Value = new();
}
