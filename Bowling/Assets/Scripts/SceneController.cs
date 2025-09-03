using UnityEngine;
//�V�[���Ǘ��@�\
using UnityEngine.SceneManagement;
//�^���S�ȃR���N�V����
using System.Collections.Generic;
//�񓯊���������񏈗��Ǘ��@�\
using System.Threading.Tasks;

public class SceneController : MonoBehaviour
{
    //�V���O���g��
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        //�C���X�^���X����ł����
        if(Instance == null)
        {
            Instance = this; DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
