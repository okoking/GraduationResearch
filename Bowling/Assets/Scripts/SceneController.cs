using UnityEngine;
//シーン管理機能
using UnityEngine.SceneManagement;
//型安全なコレクション
using System.Collections.Generic;
//非同期処理や並列処理管理機能
using System.Threading.Tasks;

public class SceneController : MonoBehaviour
{
    //シングルトン
    public static SceneController instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
