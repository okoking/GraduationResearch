using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/MissionData")]

public class MissionData : ScriptableObject
{
    public string missionText;       //ミッション時に表示するテキスト
    public int    missionClearValue; //ミッションをクリアしたときのメーター増加量
}