using TMPro;
using UnityEngine;

public class HintButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintAmountTMP;
    private GameData Data => AssetProvider.Instance.Data.Value;
    public void UpdateHintText()
    {
        hintAmountTMP.text = Data.HintAmount.ToString();
    }
}
