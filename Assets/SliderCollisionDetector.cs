using UnityEngine;
using UnityEngine.UI;

public class SliderCollisionDetector : MonoBehaviour
{
    [SerializeField] private Image collectible;
    [SerializeField] private RectTransform handle;
    [SerializeField] private WinningManager winningManager;
    public void Initialize(Sprite sprite)
    {
        collectible.enabled = true;
        collectible.sprite = sprite;
    }

    void Update()
    {
        if (collectible.enabled && IsColliding(collectible.rectTransform, handle))
        {
            collectible.enabled = false;
            winningManager.OnCollectibleCollected();
        }
    }



    bool IsColliding(RectTransform rectTransform1, RectTransform rectTransform2)
    {
        Rect rect1 = GetWorldRect(rectTransform1);
        Rect rect2 = GetWorldRect(rectTransform2);
        return rect1.Overlaps(rect2);
    }

    Rect GetWorldRect(RectTransform rt)
    {
        // Convert the local RectTransform to world space Rect
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        return new Rect(bottomLeft, topRight - bottomLeft);
    }
}