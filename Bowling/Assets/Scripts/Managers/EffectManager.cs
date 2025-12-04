using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    // 名前でエフェクトPrefabを取得する辞書
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Resources/Effects の下にPrefabを置いておく
        effects["meteor"] = Resources.Load<GameObject>("Effects/Meteors AOE");
    }

    /// <summary>
    /// エフェクト生成リクエスト
    /// </summary>
    /// <param name="effectName">Startで登録した文字列</param>
    /// <param name="pos">生成座標</param>
    public void Request(string effectName, Vector3 pos)
    {
        if (effects.ContainsKey(effectName) && effects[effectName] != null)
        {
            GameObject go = Instantiate(effects[effectName], pos, Quaternion.identity);

            // エフェクトの再生が終わったら自動で消したい場合
            ParticleSystem ps = go.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(go, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                // 自動削除したくない場合はコメントアウト
                Destroy(go, 5f);
            }
        }
        else
        {
            Debug.LogWarning($"Effect {effectName} が登録されていません");
        }
    }
}
