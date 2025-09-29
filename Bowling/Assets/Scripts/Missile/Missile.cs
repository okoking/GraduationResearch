using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    //�^�[�Q�b�g
    Transform target;
    [SerializeField, Min(0)]
    //���b�Œ��e�����邩
    float time = 1;
    [SerializeField]
    //�����\����
    float lifeTime = 2;
    [SerializeField]
    bool limitAcceleration = false;
    [SerializeField, Min(0)]
    float maxAcceleration = 100;
    [SerializeField]
    //�ŏ��U�ꕝ
    Vector3 minInitVelocity;
    [SerializeField]
    //�ő�U�ꕝ
    Vector3 maxInitVelocity;

    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;
    Transform thisTransform;

    public Transform Target
    {
        set
        {
            target = value;
        }
        get
        {
            return target;
        }
    }

    void Start()
    {
        thisTransform = transform;
        position = thisTransform.position;
        //���̋O���̏����U�ꕝ
        velocity = new Vector3(Random.Range(minInitVelocity.x, maxInitVelocity.x), Random.Range(minInitVelocity.y, maxInitVelocity.y), Random.Range(minInitVelocity.z, maxInitVelocity.z));
        target = FindRandomTarget();

        StartCoroutine(nameof(Timer));
    }

    public void Update()
    {
        //�^�[�Q�b�g���Ȃ���΃~�T�C������
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        //�����x���v�Z
        acceleration = 2f / (time * time) * (target.position - position - time * velocity);

        if (limitAcceleration && acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        time -= Time.deltaTime;

        if (time < 0f)
        {
            return;
        }

        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        thisTransform.position = position;
        thisTransform.rotation = Quaternion.LookRotation(velocity);
    }


    IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }

    Transform FindRandomTarget()
    {
        //�G�̃^�O���t�����I�u�W�F�N�g��T���Ċi�[
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        //�^�[�Q�b�g�Ƃ̋�����0�Ȃ�null��Ԃ�
        if (targets.Length == 0) return null;
        //�����_���ȃ^�[�Q�b�g�̃g�����X�t�H�[����Ԃ�
        return targets[Random.Range(0, targets.Length)].transform;
    }
}
