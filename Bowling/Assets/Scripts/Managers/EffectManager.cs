using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    //登録しておくエフェクトPrefabリスト
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    //再生中のエフェクトリスト
    private Dictionary<string, List<int>> effectsByName = new Dictionary<string, List<int>>();

    //再生中エフェクトIDリスト
    private Dictionary<int, GameObject> effectsById = new Dictionary<int, GameObject>();

    //ID用カウンタ
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

    //エフェクトの登録を行う。
    //エフェクトはResourcesファイルの中のEffectsの中に入れる
    //配列の名前は被らない限り自由な名前でOK
    void Start()
    {
        //      kore↓名前                                         ↓エフェクトの名前
        effects["meteor"] = Resources.Load<GameObject>("Effects/Meteors AOE");
        effects["BeamColl"] = Resources.Load<GameObject>("Effects/AoE slash blue");
    }

    //エフェクトの呼び出し
    //引数(Startでつけた名前/再生する座標)
    //使用方法 : EffectManager.instance.Play(名前,座標) : 複数呼び出しも可能
    public int Play(string effectName, Vector3 pos)
    {
        //登録されていなければエラー
        if (!effects.ContainsKey(effectName)) return -1;

        //生成する
        GameObject fx = Instantiate(effects[effectName], pos, Quaternion.identity);

        int id = nextId++;
        effectsById[id] = fx;   //生成順にIDを付与する(個別でエフェクトを管理するため)
            
        //名前単位でリスト管理
        if (!effectsByName.ContainsKey(effectName))
            effectsByName[effectName] = new List<int>();

        effectsByName[effectName].Add(id);

        return id;
    }

    //Playで割り振られたIDで個別にエフェクトを停止
    //引数(Playで割り振られたID(Playでの戻り値))
    //使用方法 : EffectMaanger.instace.Pause(ID)
    public void Pause(int id)
    {
        if (!effectsById.ContainsKey(id)) return;

        var ps = effectsById[id].GetComponent<ParticleSystem>();
        if (ps != null) ps.Pause();
    }

    //その名前を付けたエフェクト全てを停止
    //引数(Startでつけた名前)
    //使用方法 : EffectManager.instance.Pause("名前")
    public void Pause(string effectName)
    {
        if (!effectsByName.ContainsKey(effectName)) return;

        foreach (var id in effectsByName[effectName])
            Pause(id);
    }

    //Playで割り振られたIDで個別にエフェクトを再生
    //引数(Playで割り振られたID(Playでの戻り値))
    //使用方法 : EffectManager.instance.Resume(ID)
    public void Resume(int id)
    {
        if (!effectsById.ContainsKey(id)) return;

        var ps = effectsById[id].GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    //その名前を付けたエフェクト全てを再生
    //引数(Startでつけた名前)
    //使用方法 : EffectManager.instance.Resume("名前")
    public void Resume(string effectName)
    {
        if (!effectsByName.ContainsKey(effectName)) return;

        foreach (var id in effectsByName[effectName])
            Resume(id);
    }

    //Playで割り振られたIDで個別にエフェクトを停止し破棄
    //引数(Playで割り振られたID(Playでの戻り値))
    //使用方法 : EffectMaanger.instace.Stop(ID)
    public void Stop(int id)
    {
        if (!effectsById.ContainsKey(id)) return;

        GameObject fx = effectsById[id];
        var ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Destroy(fx, 0.1f);

        effectsById.Remove(id);

        //名前リストからも削除
        foreach (var list in effectsByName.Values)
            list.Remove(id);
    }

    //その名前を付けたエフェクト全てを停止し破棄
    //引数(Startでつけた名前)
    //使用方法 : EffectManager.instance.Stop("名前")
    public void Stop(string effectName)
    {
        if (!effectsByName.ContainsKey(effectName)) return;

        foreach (var id in new List<int>(effectsByName[effectName]))
            Stop(id);

        effectsByName[effectName].Clear();
    }

    //生成した全てのエフェクトを削除
    //引数(なし)
    //使用方法 : EffectManager.instance.StopAll()
    public void StopAll()
    {
        foreach (var id in new List<int>(effectsById.Keys))
            Stop(id);

        effectsByName.Clear();
    }
}
