using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text missionTitle;
    [SerializeField] private TMP_Text missionDescription;
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private float showTime = 3f; // •\Ž¦ŽžŠÔ

    public void ShowMission(MissionData data)
    {
        missionTitle.text = $"Mission: {data.missionText}";
        missionDescription.text = data.missionDescription;
        missionPanel.SetActive(true);
        CancelInvoke();
        Invoke(nameof(HidePanel), showTime);
    }

    public void ShowMissionClear(MissionData data)
    {
        missionTitle.text = $"Mission: {data.missionText}";
        missionDescription.text = $"a: {data.missionClearValue}a";
        missionPanel.SetActive(true);
        CancelInvoke();
        Invoke(nameof(HidePanel), showTime);
    }

    private void HidePanel()
    {
        missionPanel.SetActive(false);
    }
}