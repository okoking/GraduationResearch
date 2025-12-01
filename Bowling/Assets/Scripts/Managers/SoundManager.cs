using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    private Dictionary<string,AudioClip> sounds = new Dictionary<string,AudioClip>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //配列を文字列で扱う。文字列は何でもよくて、SE/BGMのパスと結びつける
        sounds["LevelUp"] = Resources.Load<AudioClip>("Sounds/SE/LevelUp");
        sounds["PokuPoku"] = Resources.Load<AudioClip>("Sounds/SE/Poku");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    //Startで設定した文字列,再生座標
    public void Request(string soundName,Vector3 pos)
    {
        if (sounds.ContainsKey(soundName)&& sounds[soundName] != null)
        {
            AudioSource.PlayClipAtPoint(sounds[soundName], pos);
        }
    }

    //使いたいとこで👇こう呼ぶ
    //SoundManager.instance.Request("LevelUp", transform.position);
}