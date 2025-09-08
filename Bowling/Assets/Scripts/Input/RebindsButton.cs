using UnityEngine;
//UI�n�N���X
using UnityEngine.UI;
using TMPro;

public class RebindsButton : MonoBehaviour
{
    [SerializeField] private string actionName;     //"Player/Jump" �Ȃ�
    [SerializeField] private int bindingIndex;      //�����o�C���h�Ή��p
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text bindingText;      //���݂̃L�[�\���p

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�ŏ��̕\��
        UpdateUI();
        //�{�^���N���b�N��
        button.onClick.AddListener(() =>
        {
            if (bindingText == null)
            {
                Debug.LogError("bindingText �����蓖�Ă��Ă��܂���");
                return;
            }
            if (InputManager.Instance == null)
            {
                Debug.LogError("InputManager ���V�[���ɑ��݂��܂���");
                return;
            }

            var action = InputManager.Instance.GetAction(actionName);
            if (action == null)
            {
                Debug.LogError($"�A�N�V���� {actionName} ��������܂���");
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
