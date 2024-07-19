using LeTai.Asset.TranslucentImage;
using Sirenix.OdinInspector;
using UnityEngine;

public class TransluscentController : MonoBehaviour
{
    [SerializeField] private TranslucentImageSource TranslucentSource;
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;
    
    [Button]
    public void SwitchToGlass()
    {
        SwitchTransluscentType(TransluscentType.Glass);
    }

    [Button]
    public void SwitchToBlur()
    {
        SwitchTransluscentType(TransluscentType.Blur);
    }

    private void SwitchTransluscentType(TransluscentType transluscentType)
    {
        var blurSettings = (ScalableBlurConfig)TranslucentSource.BlurConfig;

        switch (transluscentType)
        {
            case TransluscentType.Glass:
                blurSettings.Iteration = AnimData.TransluscentGlassConfig.x;
                TranslucentSource.Downsample = AnimData.TransluscentGlassConfig.y;
                break;
            case TransluscentType.Blur:
                blurSettings.Iteration = AnimData.TransluscentBlurConfig.x;
                TranslucentSource.Downsample = AnimData.TransluscentBlurConfig.y;
                break;
        }
    }
}

public enum TransluscentType
{
    Glass,
    Blur,
}
