using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform startPoint;    // Starting point of the curve
    public Transform controlPoint;  // Control point to create the curve
    public Transform endPoint;      // Ending point of the curve

    public int curveResolution = 10; // Number of points on the curve

    // private void OnDrawGizmos()
    // {
    //     // Ensure all required points are assigned
    //     if (startPoint == null || controlPoint == null || endPoint == null)
    //         return;
    //
    //     // Calculate the quadratic Bezier curve points
    //     Vector3[] curvePoints = new Vector3[curveResolution + 1];
    //     for (int i = 0; i <= curveResolution; i++)
    //     {
    //         float t = i / (float)curveResolution;
    //         curvePoints[i] = CalculateBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
    //     }
    //
    //     // Draw the curve
    //     Gizmos.color = Color.white;
    //     for (int i = 0; i < curvePoints.Length - 1; i++)
    //     {
    //         Gizmos.DrawLine(curvePoints[i], curvePoints[i + 1]);
    //     }
    // }

    // Calculate a point on the quadratic Bezier curve
    
}