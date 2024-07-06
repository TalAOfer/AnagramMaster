
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private StartMenuButton StartMenuButton;
    private GameData Data => AssetProvider.Instance.Data.Value;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;

    public void Initialize()
    {
        NextLevelData nextLevelData = new(Data.IndexHierarchy, BiomeBank);
        StartMenuButton.Initialize(nextLevelData);
    }
}
