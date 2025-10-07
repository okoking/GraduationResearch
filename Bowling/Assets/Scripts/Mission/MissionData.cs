using Unity.VisualScripting;
using UnityEngine;

public enum MissionType
{
    HIT,
}

[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/MissionData")]

public class MissionData : ScriptableObject
{
    public string missionText;       //�~�b�V�������ɕ\������e�L�X�g
    public string missionDescription;//�~�b�V�����̐���
    public int    missionClearValue; //�~�b�V�������N���A�����Ƃ��̃��[�^�[������

    public MissionType missionType;  //�~�b�V�����̎��
    public int targetCount;          //��
}