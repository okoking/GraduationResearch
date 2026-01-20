using UnityEngine;

public class SoundManager : MonoBehaviour
{
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
    }

    //================================================================================

    //サウンドのリクエスト
    public void Request(string soundID, Vector3 pos)
    {
        //指定したサウンドが読み込まれていなければ実行しない
        if (m_SoundList.SoundFind(soundID).audioClip == null) return;

        //指定した座標から音を鳴らす
        AudioSource.PlayClipAtPoint(
            m_SoundList.SoundFind(soundID).audioClip, pos,
            m_SoundList.SoundFind(soundID).soundVolume);
    }
    //サウンドのリクエスト
    public void Request(string audioSourceID,string soundID)
    {
        //指定したサウンドが読み込まれていなければ実行しない
        if (m_SoundList.SoundFind(soundID).audioClip == null ||
            m_SoundList.AudioSourceFind(audioSourceID).audioSource == null) return;

        //立体音響なし再生
        m_SoundList.AudioSourceFind(audioSourceID).audioSource.PlayOneShot(
            m_SoundList.SoundFind(soundID).audioClip, 
            m_SoundList.SoundFind(soundID).soundVolume);
    }
}
