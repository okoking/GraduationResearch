using UnityEngine;
//UI�n�N���X
using UnityEngine.UI;
using TMPro;

public class RebindsButton : MonoBehaviour
{
    [SerializeField] private string actionName;     //"Player/Jump" �Ȃ�
    [SerializeField] private int bindingIndex = 0;  //�����o�C���h�Ή��p
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text bindingText;      //���݂̃L�[�\���p

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �ŏ��̕\��
        UpdateUI();
        // �{�^���N���b�N��
        button.onClick.AddListener(() =>
        {
            bindingText.text = "Press a key..."; // �ҋ@�\��

            InputManager.Instance.StartRebind(actionName, () =>
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
