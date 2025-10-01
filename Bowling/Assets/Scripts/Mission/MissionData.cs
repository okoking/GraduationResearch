using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/MissionData")]

public class MissionData : ScriptableObject
{
    public string missionText;       //�~�b�V�������ɕ\������e�L�X�g
    public int    missionClearValue; //�~�b�V�������N���A�����Ƃ��̃��[�^�[������
}