using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]Camera MainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
