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
        sounds["LevelUp"] = Resources.Load<AudioClip>("Sounds/SE/LevelUp");
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

    public void Request(string soundName,Vector3 pos)
    {
        if (sounds.ContainsKey(soundName)&& sounds[soundName] != null)
        {
            AudioSource.PlayClipAtPoint(sounds[soundName], pos);
        }
    }
}
