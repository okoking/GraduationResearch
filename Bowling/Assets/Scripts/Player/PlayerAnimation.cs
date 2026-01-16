using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private int layerIndex;
    private int oldlayerIndex;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetInteger("animNum", layerIndex);
        animator.SetTrigger("animTrigger");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            layerIndex = 4;
        }


        if (oldlayerIndex != layerIndex)
        {
            animator.SetInteger("animNum", layerIndex);
            animator.SetTrigger("animTrigger");
        }

        oldlayerIndex = layerIndex;
    }

    public void OnAnimEnd()
    {

    }

    public void ChangedAnim(int Index)
    {
        layerIndex = Index;
    }
}
