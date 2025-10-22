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
        normalCam.gameObject.SetActive(!isSootBeam);
        beamCam.gameObject.SetActive(isSootBeam);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            isSootBeam = !isSootBeam;
            normalCam.gameObject.SetActive(!isSootBeam);
            beamCam.gameObject.SetActive(isSootBeam);
            Debug.Log("X");
        }
    }
}
