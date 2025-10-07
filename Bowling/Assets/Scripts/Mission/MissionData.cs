using Unity.VisualScripting;
using UnityEngine;

public enum MissionType
{
    HIT,
}

[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/MissionData")]

public class MissionData : ScriptableObject
{
    public string missionText;       //ミッション時に表示するテキスト
    public string missionDescription;//ミッションの説明
    public int    missionClearValue; //ミッションをクリアしたときのメーター増加量

    public MissionType missionType;  //ミッションの種類
    public int targetCount;          //数
}