using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HitSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    //タグ情報と音のデータ
    [System.Serializable]
    public struct SoundHitData 
    {
        public string tagName;
        public AudioClip hitSE;
    }

    //データのリスト
    public List<SoundHitData> soundDataList =new List<SoundHitData>();

    private void OnTriggerEnter(Collider other)
    {
        foreach (var _data in soundDataList)
        {
            if (other.CompareTag(_data.tagName))
            {
                audioSource.PlayOneShot(_data.hitSE);
            }
        }
    }

}
