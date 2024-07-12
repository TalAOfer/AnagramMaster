using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gift
{
    public List<GiftItem> Items = new();
    
    public Gift(GiftBlueprint blueprint)
    {
        Items = new (blueprint.Items);
    }

    public Gift(List<GiftItem> items)
    {
        Items = new (items);
    }

    public Gift Clone()
    {
        return new Gift(Items);
    }
}

[System.Serializable]
public struct GiftItem
{
    public GiftType GiftType;
    public int GiftAmount;
}
public enum GiftType 
{
    Hint,
}
