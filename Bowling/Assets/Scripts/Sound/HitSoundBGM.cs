using System.Collections.Generic;
using UnityEngine;

public class HitSoundBGM : MonoBehaviour
{
  [SerializeField] private string areaName;

    //すでに踏まれている・一回だけ再生するように
    bool PushedArea1 = true;    //これはシーン初めに再生されている
    bool PushedArea2 = false;
    bool PushedAreaBoss = false;

    //文字を表示する
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (areaName == "第1エリア" && !PushedArea1)
            {
                SoundManager.Instance.Request("BGMPlayFirstArea");
                PushedArea1 = true;
            }
            else if (areaName == "第2エリア" && !PushedArea2)
            {
                SoundManager.Instance.Stop("BGMPlayFirstArea", true);
                PushedArea1 = false;

                SoundManager.Instance.Request("BGMPlaySecondArea");
                PushedArea2 = true;
            }
            else if (areaName == "第3エリア" && !PushedAreaBoss)
            {
                SoundManager.Instance.Stop("BGMPlaySecondArea",true);
                PushedArea2 = false;

                SoundManager.Instance.Request("BGMPlayBossArea");
                PushedAreaBoss = true;
            }
        }
    }
    ////足のキューブとエリアの感圧版に触れたら
    //private void OnTriggerEnter(Collider other)
    //{
    //    //すでに再生していたら再生しない
    //    foreach (var _data in soundDataList)
    //    {
    //        if (other.CompareTag(_data.tagName))
    //        {
    //            //連続で再生される
    //            //audioSource.clip = _data.hitBGM;
    //            //audioSource.Play();
    //            //if (!audioSource.isPlaying)
    //            //{
    //            //    audioSource.PlayOneShot(_data.hitBGM);
    //            //}
    //            break;
    //        }
    //    }
    //}
}
