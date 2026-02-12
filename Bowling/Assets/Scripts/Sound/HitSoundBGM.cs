using System.Collections.Generic;
using UnityEngine;

public class HitSoundBGM : MonoBehaviour
{
    public AudioSource audioSource;

    //タグ情報と音のデータ
    [System.Serializable]
    public struct SoundHitData
    {
        public string tagName;
        public AudioClip hitBGM;
    }

    //データのリスト
    public List<SoundHitData> soundDataList = new List<SoundHitData>();


    //足のキューブとエリアの感圧版に触れたら
    private void OnTriggerEnter(Collider other)
    {
        //すでに再生していたら再生しない
        foreach (var _data in soundDataList)
        {
            if (other.CompareTag(_data.tagName))
            {
                //連続で再生される
                //audioSource.clip = _data.hitBGM;
                //audioSource.Play();
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.PlayOneShot(_data.hitBGM);
                //}
                break;
            }
        }
    }
}
