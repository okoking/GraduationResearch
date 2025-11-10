using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PyramidMesh : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float edgeLength = 1f;
    [SerializeField] private bool pointDown = false;
    [SerializeField] private Color pyramidColor = Color.red;
    [SerializeField] private bool isLerpColor = false;
    [SerializeField] private float LerpSpeed = 1.5f;
    [SerializeField] private Color LerpColor1;
    [SerializeField] private Color LerpColor2;

    private MeshRenderer meshRenderer;
    private Material mat;

    void Start()
    {
        Mesh mesh = new Mesh();
        meshRenderer = GetComponent<MeshRenderer>();
        mat = new Material(Shader.Find("Unlit/Color")); ;
        // 高さを計算
        // 正四面体の高さ = √(2/3) × 辺の長さ
        float height = Mathf.Sqrt(2f / 3f) * edgeLength;

        // 底面をXY平面上に置き、重心が原点になるよう配置
        float r = edgeLength / (2f * Mathf.Sqrt(3f)); // 重心から底面頂点までの距離
        float baseHeight = -height / 4f; // 底面がやや下にくるように

        Vector3[] vertices = new Vector3[4];

        // 底面3点（正三角形）
        vertices[0] = new Vector3(0, baseHeight, 2f * r * Mathf.Sqrt(3f) / 3f);
        vertices[1] = new Vector3(-edgeLength / 2f, baseHeight, -r);
        vertices[2] = new Vector3(edgeLength / 2f, baseHeight, -r);

        // 頂点（上側）
        vertices[3] = new Vector3(0, baseHeight + height, 0);

        // 下向きにしたい場合
        if (pointDown)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].y = -vertices[i].y;
        }

        // 面（3角形4枚）
        int[] triangles = new int[]
        {
            0, 1, 2, // 底面
            0, 3, 1,
            1, 3, 2,
            2, 3, 0
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // --- メッシュの中心を原点に移動 ---
        Vector3 center = mesh.bounds.center;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= center; // 全頂点を中心分だけずらす
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;

        // シェーダーを頂点カラー対応のものにする
        mat.color = pyramidColor;
        meshRenderer.material = mat;
    }

    private void Update()
    {
        if (isLerpColor)
        {
            // 時間ベースで色を変える
            float t = (Mathf.Sin(Time.time * LerpSpeed) + 1f) / 2f; // 0〜1を繰り返す

            Color c = Color.Lerp(LerpColor1, LerpColor2, t); // 2色の間を往復
            meshRenderer.material.color = c;
        }
        else
        {
            mat.color = pyramidColor;
            meshRenderer.material = mat;
        }

        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}