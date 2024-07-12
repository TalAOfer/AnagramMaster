using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftUI : MonoBehaviour
{
    [SerializeField] private RectTransform GiftParent;
    [SerializeField] private RectTransform GiftTop;
    [SerializeField] private HorizontalLayoutGroup horizontalGroup;
    [SerializeField] private PositionUIOutsideScreen positionUIOutsideScreen;
    [SerializeField] List<GiftItemUI> PremadeItems = new();
    readonly List<GiftItemUI> _activeItems = new();
    private AnimationData AnimationData => AssetProvider.Instance.AnimationData;


    [FoldoutGroup("Test")]
    [SerializeField] private Gift testGift;
    [FoldoutGroup("Test")]
    [Button]
    public void Test()
    {
        GiftTop.anchoredPosition = Vector3.zero;
        positionUIOutsideScreen.PositionOutsideScreen(Sirenix.Utilities.Direction.Top);
        Initialize(testGift);
        StartCoroutine(OpenGiftAnimation());
    }

    [SerializeField] private Sprite hintSprite;
    public void Initialize(Gift gift)
    {
        _activeItems.Clear();

        for (int i = 0; i < PremadeItems.Count; i++)
        {
            if (i < gift.Items.Count)
            {
                _activeItems.Add(PremadeItems[i]);
                PremadeItems[i].gameObject.SetActive(true);
            }

            else
            {
                PremadeItems[i].gameObject.SetActive(false);
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
        yield return BringGiftToCenter();

        yield return AnimationData.GiftPreparationAnimation.GetSequence(GiftParent).Play().WaitForCompletion();
        
        yield return OpenTop();

        yield return GetItemsOut();
    }

    private IEnumerator BringGiftToCenter()
    {
        Vector2 giftCenterPoint = new(0, AnimationData.giftParentDistanceFromCenter);
        yield return GiftParent.DOAnchorPos(giftCenterPoint, AnimationData.giftAppearanceDuration)
            .SetEase(AnimationData.giftAppearanceEase)
            .WaitForCompletion();
    }

    private IEnumerator OpenTop()
    {
        yield return AnimationData.GiftOpeningAnimation.GetSequence(GiftTop).Play();
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
