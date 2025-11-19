using UnityEngine;

public class AlertEffect : MonoBehaviour
{
    private float life = 1.0f;
    private float speed = 1.0f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        life -= Time.deltaTime * 0.5f;

        ////上にふわっと動く
        //transform.position += Vector3.up * speed * Time.deltaTime;

        //フェードアウト
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Clamp01(life);
            sr.color = c;
        }

        //最大まで透過したらオブジェクトを削除
        if (life <= 0f) Destroy(gameObject);
    }
}
