using Sirenix.OdinInspector;
using UnityEngine;

//[CreateAssetMenu(menuName ="Events/Registry")]
public class EventRegistry : ScriptableObject
{
    public GameEvent OnStartButtonPressed;

    [Title("Ads/Interstitial")]
    public GameEvent Ad_LoadInterstitial;
    public GameEvent Ad_ShowInterstitial;
    public GameEvent Ad_UserClosedInterstitial;
    [Title("Ads/Banner")]
    public GameEvent Ad_LoadBanner;
    public GameEvent Ad_DisplayBanner;
    public GameEvent Ad_HideBanner;
    [Title("Ads/Rewarded")]
    public GameEvent Ad_LoadRewarded;
    public GameEvent Ad_ShowRewarded;
    public GameEvent Ad_RewardSucceeded;
}
