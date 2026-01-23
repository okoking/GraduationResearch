using UnityEngine;

public class BossAnimController : MonoBehaviour
{
    Animator animator;
    public BossState State { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        ChangeState(BossState.Idle);
    }

    public void ChangeState(BossState newState)
    {
        State = newState;
        animator.SetInteger("State", (int)newState);
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }
}
