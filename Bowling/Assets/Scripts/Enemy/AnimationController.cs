using UnityEngine;
public enum AnimState
{
    Idle,
    Move,
    Attack,
    Hit,
    
}

public class AnimationController : MonoBehaviour
{
    Animator animator;
    AnimState current;

    public AnimationController(Animator animator)
    {
        this.animator = animator;
        current = AnimState.Idle;
    }

    public void Play(AnimState next, float fade = 0.1f)
    {
        if (current == next) return;

        animator.CrossFade(next.ToString(), fade);
        current = next;
    }

    public bool IsPlaying(AnimState state)
    {
        return animator.GetCurrentAnimatorStateInfo(0)
            .IsName(state.ToString());
    }
}
