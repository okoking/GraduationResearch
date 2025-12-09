using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoidsDebugUI : MonoBehaviour
{
    [SerializeField] private EnemyAI target; // 1体だけ操作（任意で全体共有も可）

    [Header("UI Prefabs")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Slider separationSlider;
    [SerializeField] private Slider alignmentSlider;
    [SerializeField] private Slider cohesionSlider;
    [SerializeField] private Slider neighborSlider;
    [SerializeField] private Slider maxForceSlider;

    [SerializeField] private TMP_Text separationText;
    [SerializeField] private TMP_Text alignmentText;
    [SerializeField] private TMP_Text cohesionText;
    [SerializeField] private TMP_Text neighborText;
    [SerializeField] private TMP_Text maxForceText;

    [System.Obsolete]
    void Start()
    {
       
    }
}
