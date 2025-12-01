using Unity.Cinemachine;
using UnityEngine;

public class BeamCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Canvas reticle;
    [SerializeField] private CinemachineCamera normalCam;
    [SerializeField] private CinemachineCamera beamCam;
    public bool isSootBeam = false;

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
            reticle.gameObject.SetActive(isSootBeam);

            normalCam.gameObject.SetActive(!isSootBeam);
            beamCam.gameObject.SetActive(isSootBeam);
        }

        //if (isSootBeam)
        //{
        //    // ƒJƒƒ‰‚Ì‘O•ûŒü‚ðŠî€‚ÉAYŽ²‚Ì‚Ý‚ÅŒü‚«‚ð‡‚í‚¹‚é
        //    Vector3 lookDirection = cameraTransform.forward;
        //    lookDirection.y = 0f; // ã‰º‚Í–³Ž‹‚µ‚Ä…•½‰ñ“]‚Ì‚Ý
        //    if (lookDirection.sqrMagnitude > 0f)
        //    {
        //        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        //    }
        //}
    }
}
