using System.Collections;
using TMPro;
using UnityEngine;

public class WinningManager : MonoBehaviour
{
    [SerializeField] private StartMenuManager startMenuManager;
    [SerializeField] private WinningButton nextLevelButton;
    [SerializeField] private Color nativePanelTextColor;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private AnimalUIController animalUIController;
    [SerializeField] private GiftUIController giftUIController;

    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;
    private GameData Data => AssetProvider.Instance.Data.Value;

    public void Initialize(GameData dataClone)
    {
        levelText.text = "Level " + (dataClone.OverallLevelIndex + 1).ToString();

        giftUIController.Initialize(dataClone);
        animalUIController.Initialize(dataClone);

        NextLevelData nextLevelData = new(Data.IndexHierarchy, BiomeBank);
        nextLevelButton.Initialize(nextLevelData);
        
        if (nextLevelData.NextLevelEvent is NextLevelEvent.NewArea or NextLevelEvent.NewBiome)
        {

        }
    }

    public IEnumerator WinningRoutine()
    {
        yield return AnimationData.G_Fade_Out.PlayAndWait();

        yield return AnimationData.W_Fade_In.PlayAndWait();

        yield return giftUIController.GiftRoutine();

        yield return animalUIController.AnimalRoutine();

        AnimationData.W_Button_Fade_In.Play();
    }
}
