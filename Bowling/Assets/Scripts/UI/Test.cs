using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Test : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI label;

    [Range(0, 9999)]
    [SerializeField]
    int count;

    char[] characters = new char[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var value = count;
        for (int i = characters.Length - 1; i >= 0; i--)
        {
            characters[i] = (char)((value % 10) + '0');
            value /= 10;
        }

        label.SetCharArray(characters, 0, characters.Length);
    }
}
