using UnityEngine;
//���̓V�X�e��
using UnityEngine.InputSystem;
//�t�@�C���E�f�B���N�g������p
using System.IO;

public class InputManager : MonoBehaviour
{
    //�V���O���g��
    public static InputManager Instance { get; private set; }

    //�C���v�b�g�L�[���
    private GameInput inputActions;

    private string savePath;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
