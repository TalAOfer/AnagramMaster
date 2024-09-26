using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    private EventRegistry Events => AssetProvider.Instance.Events;

    private bool interStitialClosed;
    public void SetInterstitialAsClosed() { interStitialClosed = true; }

    public void Initialize(GameData dataClone)
    {
        levelText.text = "Level " + (dataClone.OverallLevelIndex + 1).ToString();

        giftUIController.Initialize(dataClone);
        animalUIController.Initialize(dataClone);

        NextLevelData nextLevelData = new(Data.IndexHierarchy, BiomeBank);
        nextLevelButton.Initialize(nextLevelData);
    }

    public IEnumerator WinningRoutine()
    {
        yield return AnimationData.G_Fade_Out.PlayAndWait();

        //Events.Ad_HideBanner.Raise();

        if (Data.OverallLevelIndex - 2 != 0 && (Data.OverallLevelIndex - 2) % 3 == 0)
        {
            interStitialClosed = false;
            Events.Ad_ShowInterstitial.Raise();
            yield return WaitForInterstitial();
        }

        yield return AnimationData.W_Fade_In.PlayAndWait();

        yield return giftUIController.GiftRoutine();

        yield return animalUIController.AnimalRoutine();

        AnimationData.W_Button_Fade_In.Play();
    }

    public IEnumerator WaitForInterstitial()
    {
        while (!interStitialClosed)
        {
            yield return null;
        }

        yield break;
    }
}
