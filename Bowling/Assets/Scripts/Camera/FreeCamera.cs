using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f;      //移動速度
    public float boostMultiplier = 3f; //Shift で加速
    public float lookSpeed = 2f;       //視点移動速度

    private float yaw;
    private float pitch;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        //カーソルをロックしてマウス視点操作を有効にする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //マウスで視点回転
        yaw += Input.GetAxis("Mouse X") * lookSpeed;
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        pitch = Mathf.Clamp(pitch, -89f, 89f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        //移動入力
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f);

        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),   //A/D
            0,
            Input.GetAxis("Vertical")      //W/S
        );

        //カメラの向き基準で移動
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 direction = forward * move.z + right * move.x;

        //上下移動（スペースとCtrl）
        if (Input.GetKey(KeyCode.Q)) direction += Vector3.up;
        if (Input.GetKey(KeyCode.E)) direction += Vector3.down;

        transform.position += direction * speed * Time.deltaTime;
    }
}

