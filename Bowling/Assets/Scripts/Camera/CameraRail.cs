using UnityEngine;

public class CameraRail : MonoBehaviour
{
    public Transform[] points;

    public Transform GetPoint(int index)
    {
        return points[index];
    }

    public int Count => points.Length;
}
