using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftUIController : MonoBehaviour
{
    [SerializeField] private GiftUI GiftUI;

    [SerializeField] private TextMeshProUGUI giftProgressionText;
    [SerializeField] private Slider giftSlider;
    private Vector2Int GiftCount;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;
    private GameData _data;

    private float GetGiftSliderFill() => (float)GiftCount.x / GiftCount.y;
    private string GetGiftAmountString() => GiftCount.x.ToString() + "/" + GiftCount.y.ToString();
    public void Initialize(GameData data)
    {
        _data = data;
        InitializeGiftBar();
    }

    private void InitializeGiftBar()
    {
        int currentGiftAmount = _data.GiftProgressionAmount;
        int targetGiftAmount = _data.GiftTargetAmount;
        GiftCount = new(currentGiftAmount, targetGiftAmount);
        giftSlider.value = GetGiftSliderFill();
        giftProgressionText.text = GetGiftAmountString();
    }

    public IEnumerator GiftRoutine()
    {
        GiftCount.x += 1;
        SoundManager.PlaySound("GiftBarFill", Vector3.zero);
        Tween tween = giftSlider.DOValue(GetGiftSliderFill(), AnimationData.sliderFillDuration).SetEase(AnimationData.sliderFillEase);
        StartCoroutine(CoroutineRunner.Instance.RunFunctionDelayed
            (AnimationData.sliderFillDuration / 2f,
            () => giftProgressionText.text = GetGiftAmountString()
        ));

        yield return tween.WaitForCompletion();

        if (GiftCount.x == GiftCount.y)
        {
            GiftUI.Initialize(_data.Gift);
            yield return GiftUI.OpenGiftAnimation();
        }
    }
}
