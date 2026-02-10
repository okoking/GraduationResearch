using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class InputImage : MonoBehaviour
{
    [SerializeField][Header("‰Ÿ‚·ƒL[")] InputAction inputKey;
    [SerializeField] CanvasFader back;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputKey.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputKey.WasPressedThisFrame())
        {
            //‰Ÿ‚µ‚Ä‚¢‚é
            back.FadeIn(0.02f);
        }
        if (inputKey.WasReleasedThisFrame())
        {
            //‰Ÿ‚µ‚Ä‚¢‚È‚¢
            back.FadeOut(0.05f);
        }
    }
}
