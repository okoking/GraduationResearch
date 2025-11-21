using UnityEngine;

public class LineHitCheck : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Update()
    {
        if (lineRenderer == null || lineRenderer.positionCount < 2) return;

        Vector3 start, end;

        // Use World Space の状態に応じて使い分け
        if (lineRenderer.useWorldSpace)
        {
            start = lineRenderer.GetPosition(0);
            end = lineRenderer.GetPosition(1);
        }
        else
        {
            start = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(0));
            end = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(1));
        }

        Vector3 dir = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        if (Physics.Raycast(start, dir, out RaycastHit hit, distance))
        {
            Debug.Log("当たったオブジェクト: " + hit.collider.name);
            Debug.DrawLine(start, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(start, end, Color.green);
        }
    }
}