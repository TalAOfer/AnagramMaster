using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftUI : MonoBehaviour
{

    [SerializeField] private TweenableElement giftParent;
    [SerializeField] private TweenableElement giftTop;
    [SerializeField] private HorizontalLayoutGroup horizontalGroup;
    [SerializeField] private PositionUIOutsideScreen positionUIOutsideScreen;
    [SerializeField] private List<GiftItemUI> premadeItems = new();

    readonly List<GiftItemUI> _activeItems = new();
    private bool _didTap;
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;


    [FoldoutGroup("Test")]
    [SerializeField] private Gift testGift;
    [FoldoutGroup("Test")]
    [Button]
    public void Test()
    {
        Initialize(testGift);
        StartCoroutine(OpenGiftAnimation());
    }

    [SerializeField] private Sprite hintSprite;
    public void Initialize(Gift gift)
    {
        giftTop.RectTransform.anchoredPosition = Vector3.zero;

        _activeItems.Clear();

        for (int i = 0; i < premadeItems.Count; i++)
        {
            if (i < gift.Items.Count)
            {
                _activeItems.Add(premadeItems[i]);
                premadeItems[i].gameObject.SetActive(true);
            }

            else
            {
                premadeItems[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _activeItems.Count; i++)
        {
            GiftItem currentGiftItem = gift.Items[i];
            Sprite giftItemSprite = GetGiftSprite(currentGiftItem.GiftType);
            _activeItems[i].Initialize(giftItemSprite, currentGiftItem.GiftAmount);
        }

        horizontalGroup.enabled = true;
    }

    public IEnumerator OpenGiftAnimation()
    {
        yield return AnimationData.Gift_Fade_In.PlayAndWait();

        yield return GetItemsOut();

        yield return AwaitBlackScreenInput();

        yield return AnimationData.Gift_Fade_Out.PlayAndWait();
    }

    public void OnBlackScreenTap() => _didTap = true;

    public IEnumerator AwaitBlackScreenInput()
    {
        AnimationData.Gift_Text_Fade_In.Play();
        
        _didTap = false;

        while (!_didTap)
        {
            yield return null;
        }
    }

    private IEnumerator GetItemsOut()
    {
        horizontalGroup.enabled = false;
        float itemHeight = AnimationData.defaultGiftItemHeight;
        foreach (GiftItemUI giftItem in _activeItems)
        {
            float currentHeight = giftItem.Rect.anchoredPosition.y;

            giftItem.Rect.DOAnchorPosY(currentHeight + itemHeight, AnimationData.giftItemAppearanceDuration)
                .SetEase(AnimationData.giftItemAppearanceEase);

            itemHeight += AnimationData.giftItemHeightSpacing;
            yield return Tools.GetWaitRealtime(AnimationData.giftItemDelaySpacing);
        }
    }

    private Sprite GetGiftSprite(GiftType giftType)
    {
        switch (giftType)
        {
            case GiftType.Hint:
                return hintSprite;
            default:
                Debug.LogError("Invalid gift type. Returning null");
                return null;
        }
    }
}
