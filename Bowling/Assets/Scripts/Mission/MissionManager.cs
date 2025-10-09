using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] List<MissionData> missions;        //�~�b�V������o�^
    [SerializeField] private MissionUIController ui;   // UI�Q��
    private MissionData                currentMission;  //�I�΂�Ă���~�b�V����
    private int                        progress = 0;    //�B�����I��
    private bool isMissionFlg = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PickRandomMission();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(progress);
        }
    }

    void PickRandomMission()
    {
        if (missions.Count == 0 || isMissionFlg) return;

        int index = Random.Range(0, missions.Count);
        currentMission = missions[index];
        progress = 0;

        Debug.Log(currentMission.missionText);
        ui.ShowMission(currentMission);

        isMissionFlg = true;
    }

    public void HitEnemy()
    {
        if (currentMission == null) return;
        if (currentMission.missionType != MissionType.HIT) return;

        progress++;
        CheckMission();
    }

    void CheckMission()
    {
        if (progress >= currentMission.targetCount)
        {
            Debug.Log("�N���A");
            ui.ShowMissionClear(currentMission);
            FindFirstObjectByType<MissileSpawner>().MeterPlus(currentMission.missionClearValue);
            isMissionFlg = false;
        }
    }
}