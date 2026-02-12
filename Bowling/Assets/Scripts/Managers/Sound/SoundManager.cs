using UnityEngine;
using UnityEngine.Audio;
using static TreeEditor.TreeGroup;
using static UnityEditor.PlayerSettings;

public class SoundManager : MonoBehaviour
{
    [Header("全体の音量")][SerializeField] float m_Volume = 1f;
    [Header("参照するサウンドリスト")][SerializeField] SoundList m_SoundList;

    public static SoundManager Instance { get; private set; }  //インスタンス

    private Transform LoopGroup;
    private Transform OneShotGroup;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //グループ作成
        LoopGroup = new GameObject("LoopGroup").transform;
        LoopGroup.SetParent(transform);

        OneShotGroup = new GameObject("OneShotGroup").transform;
        OneShotGroup.SetParent(transform);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //全体の音量調節
        AudioListener.volume = m_Volume;
    }

    //================================================================================

    //指定した座標から音を鳴らす
    //サウンドのリクエスト
    public void Request(string soundID, Vector3 pos, bool loopFlag = false)
    {
        //指定したサウンドが読み込まれていなければ実行しない
        if (m_SoundList.SoundFind(soundID).audioClip == null) return;

        //オーディオソースを作成
        GameObject audioSource = new GameObject(soundID);
        audioSource.AddComponent<AudioSource>();
        audioSource.transform.position = pos;
        //ループ再生の設定
        audioSource.GetComponent<AudioSource>().loop = loopFlag;
        if (loopFlag)
        {
            audioSource.transform.SetParent(LoopGroup.gameObject.transform);
        }
        else
        {
            audioSource.transform.SetParent(OneShotGroup.gameObject.transform);
        }

        //リソースを追加
        audioSource.GetComponent<AudioSource>().resource =
            m_SoundList.SoundFind(soundID).audioClip;
        //ボリュームを設定
        audioSource.GetComponent<AudioSource>().volume =
            m_SoundList.SoundFind(soundID).soundVolume;

        //指定した位置で鳴らす（0=2D, 1=3D）
        audioSource.GetComponent<AudioSource>().spatialBlend = 1f;

        //再生
        audioSource.GetComponent<AudioSource>().Play();

        if (loopFlag) return;
        //継続時間を越えたら自動的に削除
        ParticleSystem.Destroy(audioSource, m_SoundList.SoundFind(soundID).audioClip.length);
    }
    //サウンドのリクエスト
    //立体音響なし再生
    public void Request(string soundID, bool loopFlag = false)
    {
        //指定したサウンドが読み込まれていなければ実行しない
        if (m_SoundList.SoundFind(soundID).audioClip == null) return;

        //オーディオソースを作成
        GameObject audioSource = new GameObject(soundID);
        audioSource.AddComponent<AudioSource>();
        //ループ再生の設定
        audioSource.GetComponent<AudioSource>().loop = loopFlag;
        if (loopFlag)
        {
            audioSource.transform.SetParent(LoopGroup.gameObject.transform);
        }
        else
        {
            audioSource.transform.SetParent(OneShotGroup.gameObject.transform);
        }

        //リソースを追加
        audioSource.GetComponent<AudioSource>().resource = 
            m_SoundList.SoundFind(soundID).audioClip;
        //ボリュームを設定
        audioSource.GetComponent<AudioSource>().volume =
            m_SoundList.SoundFind(soundID).soundVolume;

        //再生
        audioSource.GetComponent<AudioSource>().Play();

        if (loopFlag) return;
        //継続時間を越えたら自動的に削除
        ParticleSystem.Destroy(audioSource, m_SoundList.SoundFind(soundID).audioClip.length);
    }

    //サウンドを停止
    public void Stop(string soundID, bool seStopFlag = false)
    {
        foreach (Transform child in LoopGroup)
        {
            if (child.name != soundID) continue;
            Destroy(child.gameObject);
        }

        if (!seStopFlag) return;
        foreach (Transform child in OneShotGroup)
        {
            if (child.name != soundID) continue;
            Destroy(child.gameObject);
        }
    }
    //すべてのサウンドを停止
    public void AllStop(bool seStopFlag = false)
    {
        foreach (Transform child in LoopGroup)
        {
            Destroy(child.gameObject);
        }

        if (!seStopFlag) return;
        foreach (Transform child in OneShotGroup)
        {
            Destroy(child.gameObject);
        }
    }
}
