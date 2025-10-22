using Unity.Cinemachine;
using UnityEngine;

public class BeamCamera : MonoBehaviour
{
    public CinemachineCamera normalCam;
    public CinemachineCamera beamCam;
    bool isSootBeam = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        normalCam.Priority = isSootBeam ? 0 : 10;
        beamCam.Priority = isSootBeam ? 10 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            isSootBeam = !isSootBeam;
            normalCam.Priority = isSootBeam ? 0 : 10;
            beamCam.Priority = isSootBeam ? 10 : 0;
            Debug.Log("X");
        }
    }
}
