using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Animator anim;
    private bool isOpened;
    //AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);

    enum AnimState
    {
        IDLE,
        OPEN,
    }
    AnimState animState;

    void Start()
    {
        anim = GetComponent<Animator>();
        isOpened = false;
        animState=AnimState.IDLE;
    }

    void Update()
    {
        // 仮：マウスをクリックした場合
        if (Input.GetMouseButtonDown(0))
        {
            animState = AnimState.OPEN;
        }

        switch (animState)
        { 
            //閉じたまま
            case AnimState.IDLE:
                IdleExce();
                break;
             
            //開く
            case AnimState.OPEN:
                OpenExce();
                break;
        }

    }

    //閉じたまま

    void IdleExce()
    {
        anim.Play("Armature_001|IdoliDoor");
    }
    //開く
    void OpenExce()
    {
        anim.Play("Armature_001|OpenDoor");
    }
}
