using Sirenix.Utilities;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Tools
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    private static readonly Dictionary<float, WaitForSecondsRealtime> WaitRealtimeDictionary = new Dictionary<float, WaitForSecondsRealtime>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;
        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    public static WaitForSecondsRealtime GetWaitRealtime(float time)
    {
        if (WaitRealtimeDictionary.TryGetValue(time, out var wait)) return wait;
        WaitRealtimeDictionary[time] = new WaitForSecondsRealtime(time);
        return WaitRealtimeDictionary[time];
    }

    public static Color Transparent()
    {
        Color color = Color.white;
        color.a = 0f;
        return color;
    }

    public static Vector2 GetPositionOutsideScreen(TweenableElement element, Direction direction)
    {
        Vector2 screenSize = GetScreenSizeInUnityTerms(element.Canvas);
        float screenHeight = screenSize.y;
        float screenWidth = screenSize.x;

        float rectHeight = element.RectTransform.rect.height;
        float rectWidth = element.RectTransform.rect.width;

        Vector2 rectPivot = element.RectTransform.pivot;

        Vector2 outsidePosition = element.RectTransform.anchoredPosition;

        if (direction == Direction.Top)
        {
            float topYOfScreen = screenHeight / 2;
            outsidePosition.y = topYOfScreen + (rectPivot.y * rectHeight);
        }
        else if (direction == Direction.Bottom)
        {
            float bottomYOfScreen = -screenHeight / 2;
            outsidePosition.y = bottomYOfScreen - ((1 - rectPivot.y) *  rectHeight);
        }
        else if (direction == Direction.Left)
        {
            float leftmostXOfScreen = -screenWidth / 2;
            outsidePosition.x = leftmostXOfScreen - ((1 - rectPivot.x) * rectWidth);
        }
        else if (direction == Direction.Right)
        {
            float rightmostXOfScreen = screenWidth / 2;
            outsidePosition.x = rightmostXOfScreen + (rectPivot.x * rectWidth);
        }

        return outsidePosition;
    }

    public static Vector2 GetPositionInsideScreen(TweenableElement element, Direction direction)
    {
        Vector2 screenSize = GetScreenSizeInUnityTerms(element.Canvas);
        float screenHeight = screenSize.y;
        float screenWidth = screenSize.x;

        // Get the rectTransform size
        float rectHeight = element.RectTransform.rect.height * element.RectTransform.localScale.y;
        float rectWidth = element.RectTransform.rect.width * element.RectTransform.localScale.x;

        Vector2 rectPivot = element.RectTransform.pivot;

        // Calculate the position inside the screen
        Vector2 insidePosition = element.RectTransform.anchoredPosition;

        if (direction == Direction.Top)
        {
            float topYOfScreen = screenHeight / 2;
            insidePosition.y = topYOfScreen - ((1 - rectPivot.y) * rectHeight);
        }
        else if (direction == Direction.Bottom)
        {
            float bottomYOfScreen = -screenHeight / 2;
            insidePosition.y = bottomYOfScreen + (rectPivot.y * rectHeight);
        }
        else if (direction == Direction.Left)
        {
            float leftmostXOfScreen = -screenWidth / 2;
            insidePosition.x = leftmostXOfScreen + (rectPivot.x * rectWidth);
        }
        else if (direction == Direction.Right)
        {
            float rightmostXOfScreen = screenWidth / 2;
            insidePosition.x = rightmostXOfScreen - ((1 - rectPivot.x) * rectWidth);
        }

        return insidePosition;
    }

    private static Vector2 GetScreenSizeInUnityTerms(Canvas canvas)
    {
        float screenHeightInPixels = Screen.height;
        float screenWidthInPixels = Screen.width;

        // Canvas Scaler settings
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        float match = canvasScaler.matchWidthOrHeight;

        // Calculate the scaling factor
        float scaleFactor = (match == 0)
            ? screenWidthInPixels / referenceResolution.x  // Match width
            : screenHeightInPixels / referenceResolution.y; // Match height

        // Calculate the screen size in canvas units
        float screenHeight = screenHeightInPixels / scaleFactor;
        float screenWidth = screenWidthInPixels / scaleFactor;

        return new Vector2(screenWidth, screenHeight);
    }
}