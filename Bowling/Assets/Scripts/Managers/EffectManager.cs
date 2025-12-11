using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    // 登録しておくエフェクトPrefab
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    // 再生中エフェクト（名前）→ 複数
    private Dictionary<string, List<int>> effectsByName = new Dictionary<string, List<int>>();

    // 再生中エフェクト（ID）→ GameObject
    private Dictionary<int, GameObject> effectsById = new Dictionary<int, GameObject>();

    // ユニークID用カウンタ
    private int nextId = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        // Resources/Effects 以下
        effects["meteor"] = Resources.Load<GameObject>("Effects/Meteors AOE");
    }

    // ===============================================================
    // Play（再生） → 返り値は個別ID
    // ===============================================================
    public int Play(string effectName, Vector3 pos)
    {
        if (!effects.ContainsKey(effectName)) return -1;

        GameObject fx = Instantiate(effects[effectName], pos, Quaternion.identity);

        int id = nextId++;
        effectsById[id] = fx;

        if (!effectsByName.ContainsKey(effectName))
            effectsByName[effectName] = new List<int>();

        effectsByName[effectName].Add(id);

        return id; // 個別に制御可能！
    }

    // ===============================================================
    // 個別 Pause
    // ===============================================================
    public void Pause(int id)
    {
        if (!effectsById.ContainsKey(id)) return;

        var ps = effectsById[id].GetComponent<ParticleSystem>();
        if (ps != null) ps.Pause();
    }

    // ===============================================================
    // 名前単位 Pause（Hit 全部）
    // ===============================================================
    public void Pause(string effectName)
    {
        if (!effectsByName.ContainsKey(effectName)) return;

        foreach (var id in effectsByName[effectName])
            Pause(id);
    }

    // ===============================================================
    // 個別 Resume
    // ===============================================================
    public void Resume(int id)
    {
        if (!effectsById.ContainsKey(id)) return;

        var ps = effectsById[id].GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    // ===============================================================
    // 名前単位 Resume（Hit 全部）
    // ===============================================================
    public void Resume(string effectName)
    {
        if (!effectsByName.ContainsKey(effectName)) return;

        foreach (var id in effectsByName[effectName])
            Resume(id);
    }

    // ===============================================================
    // 個別 Stop（破棄）
    // ===============================================================
    public void Stop(int id)
    {
        if (!effectsById.ContainsKey(id)) return;

        GameObject fx = effectsById[id];
        var ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Destroy(fx, 0.1f);

        effectsById.Remove(id);

        // 名前リストからも削除
        foreach (var list in effectsByName.Values)
            list.Remove(id);
    }

    // ===============================================================
    // 名前単位 Stop（Hit 全部）
    // ===============================================================
    public void Stop(string effectName)
    {
        if (!effectsByName.ContainsKey(effectName)) return;

        foreach (var id in new List<int>(effectsByName[effectName]))
            Stop(id);

        effectsByName[effectName].Clear();
    }

    // ===============================================================
    // 全部 Stop
    // ===============================================================
    public void StopAll()
    {
        foreach (var id in new List<int>(effectsById.Keys))
            Stop(id);

        effectsByName.Clear();
    }
}
