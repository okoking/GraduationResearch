using UnityEngine;

public class BeamSweepController : MonoBehaviour
{
    public float duration = 2f;
    public float sweepAngle = 90f;
    public float sweepSpeed = 90f;
    public float beamLength = 50f;
    public float beamWidth = 0.2f;
    public LayerMask groundLayer;

    public ParticleSystem beamEffect;

    private float currentAngle;
    private bool sweepingRight = true;

    private PlayerHealth playerHealth;

    void Start()
    {
        currentAngle = -sweepAngle / 2f;
        Destroy(gameObject, duration);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // スイープ回転
        float step = sweepSpeed * Time.deltaTime * (sweepingRight ? 1 : -1);
        currentAngle += step;
        if (Mathf.Abs(currentAngle) >= sweepAngle / 2f)
            sweepingRight = !sweepingRight;

        Quaternion rot = Quaternion.Euler(0, currentAngle, 0);
        Vector3 dir = rot * transform.forward;

        Vector3 start = transform.position;
        Vector3 end = start + dir * beamLength;

        if (Physics.Raycast(start, dir, out RaycastHit hit, beamLength, groundLayer))
            end = hit.point;

        float length = Vector3.Distance(start, end);

        // ===== エフェクト制御 =====
        transform.rotation = rot;

        var shape = beamEffect.shape;
        shape.scale = new Vector3(beamWidth, beamWidth, length);

        beamEffect.transform.position = start + dir * (length * 0.5f);

        // ===== 当たり判定 =====
        RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, dir, length);
        foreach (var h in hits)
        {
            if (h.collider.CompareTag("Player"))
            {
                playerHealth.TakeDamage(1);
                EffectManager.instance.Play("BeamColl", h.transform.position);
            }
        }
    }
}
