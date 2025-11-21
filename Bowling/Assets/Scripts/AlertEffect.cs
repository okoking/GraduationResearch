using UnityEngine;

public class AlertEffect : MonoBehaviour
{
    private float life = 0.5f;
    private float time = 1.0f;
    private float speed = 1.0f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        time -= Time.deltaTime;

        ////上にふわっと動く
        //transform.position += Vector3.up * speed * Time.deltaTime;

        if (time <= 0f)
        {
            life -= Time.deltaTime;
            //フェードアウト
            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Clamp01(life);
                sr.color = c;
            }
        }

        //最大まで透過したらオブジェクトを削除
        if (life <= 0f) Destroy(gameObject);
    }
}
