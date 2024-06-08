using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private Image mainBG;
    [SerializeField] private Image backupBG;
    [SerializeField] private float movementSpeed = 0.5f; // movement speed for tweening
    [SerializeField] private float fadeStartTime = 5f; // time to start fading
    [SerializeField] private float fadeDuration = 1f; // duration of the fade
    [SerializeField] private List<Sprite> bgSprites = new List<Sprite>();
    private Vector2 startingAcnhorPos = new(-800, 0);

    private float finalXPos;
    private int currentImageIndex = 0;

    private void Awake()
    {
        float screenWidth = Screen.width;
        float uiElementWidth = mainBG.rectTransform.rect.width * mainBG.rectTransform.localScale.x;
        finalXPos = -(uiElementWidth - screenWidth);
        mainBG.rectTransform.anchoredPosition = startingAcnhorPos;
        backupBG.rectTransform.anchoredPosition = startingAcnhorPos;
        mainBG.sprite = AssignNextSprite();
        backupBG.sprite = AssignNextSprite();
        backupBG.color = new Color(backupBG.color.r, backupBG.color.g, backupBG.color.b, 0);
    }

    private void Start()
    {
        StartCoroutine(StartImageScroll());
    }

    private IEnumerator StartImageScroll()
    {
        while (true)
        {
            // Tween mainBG
            mainBG.rectTransform.DOAnchorPosX(finalXPos, movementSpeed).SetEase(Ease.Linear).SetSpeedBased();
            yield return new WaitForSeconds(fadeStartTime);

            // Start crossfade and prepare the next cycle
            backupBG.rectTransform.anchoredPosition = startingAcnhorPos;
            backupBG.rectTransform.DOAnchorPosX(finalXPos, movementSpeed).SetEase(Ease.Linear).SetSpeedBased();

            mainBG.DOFade(0, fadeDuration);
            backupBG.DOFade(1, fadeDuration);

            yield return new WaitForSeconds(fadeDuration);

            // Swap references
            var temp = mainBG;
            mainBG = backupBG;
            backupBG = temp;

            // Reset the faded-out image
            backupBG.sprite = AssignNextSprite();
            backupBG.color = new Color(backupBG.color.r, backupBG.color.g, backupBG.color.b, 0); // Reset alpha

            yield return new WaitForSeconds(fadeStartTime);
        }
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