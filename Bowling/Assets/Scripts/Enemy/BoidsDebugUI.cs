using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoidsDebugUI : MonoBehaviour
{
    [SerializeField] private EnemyAI target; // 1�̂�������i�C�ӂőS�̋��L���j

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
        Cursor.visible = true;      // �J�[�\����\��
        Cursor.lockState = CursorLockMode.None; // ���b�N�����i���R�ɓ�������j
        if (target == null)
            target = FindObjectOfType<EnemyAI>();

        // �����l
        separationSlider.value = target.SeparationWeight;
        alignmentSlider.value = target.AlignmentWeight;
        cohesionSlider.value = target.CohesionWeight;
        neighborSlider.value = target.NeighborRadius;
        maxForceSlider.value = target.MaxBoidsForce;

        // �C�x���g�o�^
        separationSlider.onValueChanged.AddListener(v => { target.SeparationWeight = v; separationText.text = $"Separation: {v:F2}"; });
        alignmentSlider.onValueChanged.AddListener(v => { target.AlignmentWeight = v; alignmentText.text = $"Alignment: {v:F2}"; });
        cohesionSlider.onValueChanged.AddListener(v => { target.CohesionWeight = v; cohesionText.text = $"Cohesion: {v:F2}"; });
        neighborSlider.onValueChanged.AddListener(v => { target.NeighborRadius = v; neighborText.text = $"Neighbor: {v:F2}"; });
        maxForceSlider.onValueChanged.AddListener(v => { target.MaxBoidsForce = v; maxForceText.text = $"MaxForce: {v:F2}"; });
    }
}
