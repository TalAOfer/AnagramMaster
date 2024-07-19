using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Gift Bank")]
public class GiftBank : ScriptableObject
{
    public List<GiftBlueprint> Blueprints;
}

[System.Serializable]
public class GiftBlueprint
{
    public List<GiftItem> Items = new();
}
