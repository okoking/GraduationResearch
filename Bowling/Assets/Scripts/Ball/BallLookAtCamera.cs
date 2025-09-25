using UnityEngine;

public class BallLookAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTransform == null) return;

        // ƒJƒƒ‰‚Ì•ûŒü‚ğŒü‚©‚¹‚é
        transform.LookAt(cameraTransform);
    }
}
