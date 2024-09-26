using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private Image mainBG;
    [SerializeField] private Image backupBG;
    [SerializeField] private TweenableElement mainBgElement;
    [SerializeField] private float movementSpeed = 0.5f; // movement speed for tweening
    [SerializeField] private float fadeStartTime = 5f; // time to start fading
    [SerializeField] private float fadeDuration = 1f; // duration of the fade
    [SerializeField] private List<Sprite> bgSprites = new List<Sprite>();
    
    private Vector2 startingAcnhorPos;
    private float finalXPos;

    private int currentImageIndex = 0;

    private void Awake()
    {
        finalXPos = Tools.GetPositionInsideScreen(mainBgElement, Sirenix.Utilities.Direction.Right).x;
        startingAcnhorPos = Tools.GetPositionInsideScreen(mainBgElement, Sirenix.Utilities.Direction.Left);

        mainBG.rectTransform.anchoredPosition = startingAcnhorPos;
        backupBG.rectTransform.anchoredPosition = startingAcnhorPos;
        mainBG.sprite = AssignNextSprite();
        backupBG.sprite = AssignNextSprite();
        backupBG.color = new Color(backupBG.color.r, backupBG.color.g, backupBG.color.b, 0);
    }

    private void Start()
    {
        StartImageScroll();
    }

    private void StartImageScroll()
    {
        mainBG.rectTransform.DOAnchorPosX(finalXPos, movementSpeed).SetEase(Ease.Linear).SetSpeedBased();
        StartCoroutine(Crossfade());
    }

    private IEnumerator Crossfade()
    {
        yield return Tools.GetWaitRealtime(fadeStartTime);
        (backupBG, mainBG) = (mainBG, backupBG);
        mainBG.rectTransform.DOAnchorPosX(finalXPos, movementSpeed).SetEase(Ease.Linear);
        
        StartCoroutine(Crossfade());
        
        mainBG.DOFade(1, fadeDuration);
        Tween tween = backupBG.DOFade(0, fadeDuration);

        yield return tween.WaitForCompletion();
        backupBG.rectTransform.DOKill();
        backupBG.rectTransform.anchoredPosition = startingAcnhorPos;
        backupBG.sprite = AssignNextSprite();
    }

    private Sprite AssignNextSprite()
    {
        Sprite sprite = bgSprites[currentImageIndex];
        currentImageIndex++;

        if (currentImageIndex >= bgSprites.Count)
        {
            currentImageIndex = 0;
        }

        return sprite;
    }
}