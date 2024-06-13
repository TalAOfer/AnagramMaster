using Radishmouse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetterBankLineManager : MonoBehaviour
{
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private RectTransform circleTransform;
    private bool followMouse;
    private GameData data;

    public void Initialize(GameData data, Color color)
    {
        this.data = data;
        lineRenderer.color = color;
    }

    private void Start()
    {
        lineRenderer.points = new List<Vector2>();
    }

    public void AddPoint(Vector2 newPoint)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(circleTransform, newPoint, null, out localPoint);

        if (lineRenderer.points.Count == 0)
        {
            lineRenderer.points.Add(localPoint);
            lineRenderer.points.Add(GetMouseLocalPosition());
            followMouse = true; 
        }
        else
        {
            if (lineRenderer.points.Count < data.CurrentLetters.Length)
            {
                lineRenderer.points.Insert(lineRenderer.points.Count - 1, localPoint);
                followMouse = true;
            }
            else
            {
                lineRenderer.points[^1] = localPoint;
                followMouse = false;
            }
        }

        lineRenderer.SetVerticesDirty(); // Update the line renderer
    }

    public void RemovePoint()
    {
        if (lineRenderer.points.Count <= 2)
        {
            Reset();
        }
        else
        {                   
            if (followMouse)
            {
                lineRenderer.points.Remove(lineRenderer.points[^1]);
            }

            followMouse = true;
        }
    }

    public void Reset()
    {
        lineRenderer.points.Clear();
        followMouse = false;
        lineRenderer.SetVerticesDirty(); // Update the line renderer
    }

    private Vector2 GetMouseLocalPosition()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(circleTransform, Input.mousePosition, null, out mousePos);
        return mousePos;
    }

    private void Update()
    {
        if (followMouse)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        if (lineRenderer.points.Count > 1)
        {
            lineRenderer.points[^1] = GetMouseLocalPosition();
            lineRenderer.SetVerticesDirty(); // Update the line renderer
        }
    }
}
