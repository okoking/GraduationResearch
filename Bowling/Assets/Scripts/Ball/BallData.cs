using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "ScriptableObjects/BallData")]
public class BallData : ScriptableObject
{
    public string ballName;
    public Sprite icon;
    public float weight;
    public float speed;
}