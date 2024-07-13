using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftUI : MonoBehaviour
{
    [SerializeField] private RectTransform giftParent;
    [SerializeField] private RectTransform giftTop;
    [SerializeField] private HorizontalLayoutGroup horizontalGroup;
    [SerializeField] private PositionUIOutsideScreen positionUIOutsideScreen;
    [SerializeField] private List<GiftItemUI> premadeItems = new();
    [SerializeField] private ElementController elementController;
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
        positionUIOutsideScreen.PositionOutsideScreen(Sirenix.Utilities.Direction.Top);
        giftTop.anchoredPosition = Vector3.zero;

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
        yield return elementController.FadeGiftIn();

        yield return BringGiftToCenter();

        yield return AnimationData.GiftPreparationAnimation.GetSequence(giftParent).Play().WaitForCompletion();

        yield return OpenTop();

        yield return GetItemsOut();

        yield return AwaitBlackScreenInput();

        yield return elementController.FadeGiftOut();
    }

    public void OnBlackScreenTap() => _didTap = true;

    public IEnumerator AwaitBlackScreenInput()
    {
        StartCoroutine(elementController.FadeGiftTextIn());
        
        _didTap = false;

        while (!_didTap)
        {
            yield return null;
        }
    }
    
    private IEnumerator BringGiftToCenter()
    {
        Vector2 giftCenterPoint = new(0, AnimationData.giftParentDistanceFromCenter);
        yield return giftParent.DOAnchorPos(giftCenterPoint, AnimationData.giftAppearanceDuration)
            .SetEase(AnimationData.giftAppearanceEase)
            .WaitForCompletion();
    }

    private IEnumerator OpenTop()
    {
        yield return AnimationData.GiftOpeningAnimation.GetSequence(giftTop).Play();
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
