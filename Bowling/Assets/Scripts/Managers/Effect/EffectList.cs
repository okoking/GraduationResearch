using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectData
{
    [Tooltip("エフェクトID")] public string effectID;
    [Tooltip("エフェクト")] public GameObject effectObject;
}

[CreateAssetMenu(menuName = "System/EffectList")]
public class EffectList : ScriptableObject
{
    public List<EffectData> effectList = new List<EffectData>();

    //要素を探す
    public EffectData Find(string effectID)
    {
        return effectList.Find(effect => effect.effectID == effectID);
    }
}