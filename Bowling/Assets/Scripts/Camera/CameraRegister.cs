using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraRegister : MonoBehaviour
{
    public CameraMode mode;

    void Awake()
    {
        CameraManager.Instance.Register(
            mode,
            GetComponent<CinemachineCamera>()
        );
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
