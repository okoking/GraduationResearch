using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlasingBtn : MonoBehaviour
{
    float time;
    float speed = 1.0f;
    TMP_Text text;
   
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.color = GetAlphaColor(text.color);
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 3.5f * speed;
        color.a = Mathf.Sin(time);

        return color;
    }
}
