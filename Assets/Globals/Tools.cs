using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

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
        // Get the screen dimensions in canvas units
        float screenHeight = Screen.height / element.Canvas.scaleFactor;
        float screenWidth = Screen.width / element.Canvas.scaleFactor;

        // Get the rectTransform size
        float rectHeight = element.RectTransform.rect.height;
        float rectWidth = element.RectTransform.rect.width;

        // Calculate the position outside the screen
        Vector2 outsidePosition = element.RectTransform.anchoredPosition;

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

        return outsidePosition;
    }
}