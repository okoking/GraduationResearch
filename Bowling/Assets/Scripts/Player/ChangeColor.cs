using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private Color pyramidColor = Color.red;
    [SerializeField] private bool isLerpColor = false;
    [SerializeField] private float LerpSpeed = 1.5f;
    [SerializeField] private Color LerpColor1;
    [SerializeField] private Color LerpColor2;

    private Renderer Renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // このオブジェクトのマテリアルを取得
        Renderer = GetComponent<Renderer>();

        Renderer.material.color = pyramidColor;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isLerpColor)
        {
            // 時間ベースで色を変える
            float t = (Mathf.Sin(Time.time * LerpSpeed) + 1f) / 2f; // 0〜1を繰り返す

            Color c = Color.Lerp(LerpColor1, LerpColor2, t); // 2色の間を往復
            Renderer.material.color = c;
        }
        else
        {
            if(Renderer.material.color != pyramidColor)
            {
                Renderer.material.color = pyramidColor;
            }
        }
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
