using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    [Tooltip("サウンドID")] public string soundID;
    [Tooltip("オーディオクリップ")] public AudioClip audioClip;
    [Tooltip("音量")][Range(0.0f, 1.0f)] public float soundVolume = 1f;
    [Tooltip("備考欄")]
    [TextArea(0, 4)]
    public string comment;
}

[System.Serializable]
public class AudioSourceData
{
    [Tooltip("AudioSourceID")] public string audioSourceID;
    [Tooltip("AudioSource")] public AudioSource audioSource;
    [Tooltip("備考欄")]
    [TextArea(0, 4)]
    public string comment;
}

[CreateAssetMenu(menuName = "System/SoundList")]
public class SoundList : ScriptableObject
{
    public List<SoundData> soundList = new List<SoundData>();
    public List<AudioSourceData> audioSourceList = new List<AudioSourceData>();

    //要素を探す
    public SoundData SoundFind(string soudID)
    {
        return soundList.Find(sound => sound.soundID == soudID);
    }

    //要素を探す
    public AudioSourceData AudioSourceFind(string audioSourceID)
    {
        return audioSourceList.Find(sound => sound.audioSourceID == audioSourceID);
    }
}