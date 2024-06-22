
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private NextLevelButton StartMenuButton;
    private GameData Data => AssetProvider.Instance.Data.Value;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;

    public void Initialize()
    {
        NextLevelData nextLevelData = BiomeBank.GetNextLevelData(Data.IndexHierarchy);
        StartMenuButton.Initialize(nextLevelData);
    }
}
