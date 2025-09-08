using UnityEngine;
//UI系クラス
using UnityEngine.UI;
using TMPro;

public class RebindsButton : MonoBehaviour
{
    [SerializeField] private string actionName;     //"Player/Jump" など
    [SerializeField] private int bindingIndex;      //複数バインド対応用
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text bindingText;      //現在のキー表示用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //最初の表示
        UpdateUI();
        //ボタンクリック時
        button.onClick.AddListener(() =>
        {
            if (bindingText == null)
            {
                Debug.LogError("bindingText が割り当てられていません");
                return;
            }
            if (InputManager.Instance == null)
            {
                Debug.LogError("InputManager がシーンに存在しません");
                return;
            }

            var action = InputManager.Instance.GetAction(actionName);
            if (action == null)
            {
                Debug.LogError($"アクション {actionName} が見つかりません");
                return;
            }

            bindingText.text = "Press a key...";
            InputManager.Instance.StartRebind(actionName, 0, () =>
            {
                UpdateUI();
            });

        });
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    private void UpdateUI()
    {
        //bindingText.text = InputManager.Instance.GetBindingDisplayName(actionName, bindingIndex);
    }
}
