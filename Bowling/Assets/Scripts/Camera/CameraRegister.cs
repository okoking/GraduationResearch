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

    void Update()
    {
        
    }
}
