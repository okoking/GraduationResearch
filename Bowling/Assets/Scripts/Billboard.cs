using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform enemy;          // ’Ç]‚·‚é‘ÎÛi“Gj
    public Vector3 offset = new Vector3(0, 2f, 0); // “ªã‚Ì‚‚³
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        // “G‚Ì“ªã‚Ö’Ç]
        if (enemy != null)
            transform.position = enemy.position + offset;
    }
}
