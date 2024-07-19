using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class PositionUIOutsideScreen : MonoBehaviour
{
    // Reference to the RectTransform component
    private RectTransform rectTransform;
    private Canvas canvas;
    [SerializeField] private Direction _direction;

    [Button]
    public void ManuallyPosition()
    {
        PositionOutsideScreen(_direction);
    }

    public void PositionOutsideScreen(Direction direction)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        // Get the screen dimensions in canvas units
        float screenHeight = Screen.height / canvas.scaleFactor;
        float screenWidth = Screen.width / canvas.scaleFactor;

        // Get the rectTransform size
        float rectHeight = rectTransform.rect.height;
        float rectWidth = rectTransform.rect.width;

        // Calculate the position outside the screen
        Vector2 outsidePosition = rectTransform.anchoredPosition;

        if (direction == Direction.Top)
        {
            outsidePosition.y = screenHeight + rectHeight / 2;
        }
        else if (direction == Direction.Bottom)
        {
            outsidePosition.y = -(screenHeight + rectHeight / 2);
        }
        else if (direction == Direction.Left)
        {
            outsidePosition.x = -(screenWidth + rectWidth / 2);
        }
        else if (direction == Direction.Right)
        {
            outsidePosition.x = screenWidth + rectWidth / 2;
        }

        // Set the rectTransform position
        rectTransform.anchoredPosition = outsidePosition;
    }
}
