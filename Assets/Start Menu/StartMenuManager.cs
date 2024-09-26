
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private StartMenuButton StartMenuButton;
    private GameData Data => AssetProvider.Instance.Data.Value;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;

    private EventRegistry Events => AssetProvider.Instance.Events;

    public void Initialize()
    {
        NextLevelData nextLevelData = new(Data.IndexHierarchy, BiomeBank);
        Events.Ad_LoadRewarded.Raise();
        Events.Ad_LoadBanner.Raise();
        Events.Ad_HideBanner.Raise();
        StartMenuButton.Initialize(nextLevelData);
    }
}
