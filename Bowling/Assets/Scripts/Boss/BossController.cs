//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public enum BossAttackType
//{
//    Normal,
//    Beam,
//}


//public class BossController : MonoBehaviour
//{
//    public BossAnimController anim;
//    public BossHand hand;

//    BossAttackType currentAttack = BossAttackType.Normal;

//    void Awake()
//    {
//        anim = GetComponent<BossAnimController>();
//        hand = GetComponentInChildren<BossHand>();
//    }

//    void Start()
//    {
//        StartNextAttack();
//    }

//    public void StartNextAttack()
//    {
//        switch (currentAttack)
//        {
//            case BossAttackType.Normal:
//                StartNormalAttack();
//                currentAttack = BossAttackType.Beam;
//                break;

//            case BossAttackType.Beam:
//                StartBeamAttack();
//                currentAttack = BossAttackType.Normal;
//                break;
//        }
//    }

//    void StartBeamAttack()
//    {
//        anim.ChangeState(BossState.Attack);
//        //hand.StartBeamAttack();
//    }

//    // çUåÇÇ™èIÇÌÇ¡ÇΩÇÁåƒÇŒÇÍÇÈ
//    public void OnAttackEnd()
//    {
//        anim.ChangeState(BossState.Beam);
//        StartNextAttack(); // Å© éüÇÃçUåÇÇ÷
//    }

//    void StartNormalAttack()
//    {
//        anim.ChangeState(BossState.Attack);
//        anim.PlayAttack();
//        StartCoroutine(NormalAttackRoutine());
//    }

//    IEnumerator NormalAttackRoutine()
//    {
//        yield return new WaitForSeconds(1.0f); // í èÌçUåÇÇÃí∑Ç≥
//        OnAttackEnd();
//    }
//}