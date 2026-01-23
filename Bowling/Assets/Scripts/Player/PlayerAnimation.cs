using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private int layerIndex;
    private int oldlayerIndex;
    private Animator animator;

    private bool isShootingBeam;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetInteger("animNum", layerIndex);
        animator.SetTrigger("animTrigger");
        isShootingBeam = false;
    }


    void Update()
    {
        if (oldlayerIndex != layerIndex)
        {
            animator.SetInteger("animNum", layerIndex);
            animator.SetTrigger("animTrigger");
        }

        oldlayerIndex = layerIndex;

        if(layerIndex!=5 && isShootingBeam)
        {
            isShootingBeam = false;
        }
    }

    public void EndweekBeam()
    {
        isShootingBeam = false;
        Debug.Log("owa");
    }

    public void OnEndAnim()
    {

    }

    public void ChangedAnim(int Index)
    {
        if (isShootingBeam) return;

        layerIndex = Index;

        if (layerIndex == 5)
        {
            isShootingBeam = true;
        }
    }

    public bool GetisShootingBeam()
    {
        return isShootingBeam;
    }
}
