using UnityEngine;

public class DoorSequenceManager : MonoBehaviour
{
    [SerializeField] private DoorDeath[] doors;
    [SerializeField] private BossSpawner bossSpawner;

    private int currentIndex = 0;

    void OnEnable()
    {
        DoorDeath.OnDoorOpened += HandleDoorOpened;
    }

    void OnDisable()
    {
        DoorDeath.OnDoorOpened -= HandleDoorOpened;
    }

    void Start()
    {
        //最初のドアだけ有効、他はロック
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].gameObject.SetActive(i == 0);
        }
    }

    void HandleDoorOpened(int openedIndex)
    {
        if (openedIndex != currentIndex) return;

        currentIndex++;

        //次のドアを開放
        if (currentIndex < doors.Length)
        {
            doors[currentIndex].gameObject.SetActive(true);
        }
        else
        {
            //全ドア開いたらボス出現
            bossSpawner.SpawnBoss();
        }
    }
}
