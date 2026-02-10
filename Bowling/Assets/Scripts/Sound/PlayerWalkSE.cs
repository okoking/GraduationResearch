using UnityEngine;

public class PlayerWalkSE : MonoBehaviour
{
    [SerializeField] AudioSource seWalk;

    public void SePlay()
    {
        seWalk.Play();
    }

}
