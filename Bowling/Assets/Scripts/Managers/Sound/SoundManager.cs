using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    [Header("全体の音量")][SerializeField] float m_Volume = 1f;
    [Header("参照するサウンドリスト")][SerializeField] SoundList m_SoundList;

    public static SoundManager Instance { get; private set; }  //インスタンス

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //全体の音量調節
        AudioListener.volume = m_Volume;
        m_AudioSource.volume = m_Volume;
    }

    //================================================================================

    //サウンドのリクエスト
    public void Request(string soundID, Vector3 pos)
    {
        //指定したサウンドが読み込まれていなければ実行しない
        if (m_SoundList.Find(soundID).audioClip == null) return;

        //指定した座標から音を鳴らす
        AudioSource.PlayClipAtPoint(m_SoundList.Find(soundID).audioClip, pos, m_SoundList.Find(soundID).soundVolume);
    }
    //サウンドのリクエスト
    public void Request(string soundID)
    {
        //指定したサウンドが読み込まれていなければ実行しない
        if (m_SoundList.Find(soundID).audioClip == null) return;

        //立体音響なし再生
        m_AudioSource.PlayOneShot(m_SoundList.Find(soundID).audioClip, m_SoundList.Find(soundID).soundVolume);
    }
}
