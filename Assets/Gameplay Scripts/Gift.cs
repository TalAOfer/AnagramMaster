using UnityEngine;

[System.Serializable]
public class Gift
{
    public GiftType GiftType;
    public int GiftAmount;
    
    public Gift()
    {
        GiftType = GiftType.Hint;
        GiftAmount = 3;
    }

    public Gift Clone()
    {
        return new Gift(GiftType, GiftAmount);
    }

    private Gift(GiftType giftType, int giftAmount)
    {
        GiftType = giftType;
        GiftAmount = giftAmount;
    }
}

public enum GiftType 
{
    Hint,
}
