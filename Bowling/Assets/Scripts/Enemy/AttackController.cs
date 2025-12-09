using System.Collections.Generic;
using UnityEngine;

public class AttackController
{

    [Header("攻撃管理")]
    public int maxAttacker = 3;                     //同時に攻撃できる人数
    private int currentAttacking = 0;               //今攻撃している敵の人数
    private Queue<EnemyAI> attackQueue = new();

    [Header("攻撃テンポ調整")] 
    public float globalAttackCooldown = 2.5f;       //全体クールダウン
    private float globalAttackTimer = 0f;

    [Header("ラッシュ攻撃（複数同時攻撃）")] 
    public float rushChance = 0.1f;                 //10%の確率でラッシュ
    public float rushDuration = 4f;                 //ラッシュ継続時間
    public float rushCooldown = 10f;                //ラッシュとラッシュの間隔
    public bool isRush = false;                     //Rushするかどうか
    private float rushTimer = 0f;                   //Rush間隔
    private float rushCooldownTimer = 0f;           //Rushクールダウン

    private int baseMaxAttacker;

    public AttackController()
    {
        //MaxAttacker = baseMaxAttacker;
    }

    public void Update()
    {
        //if (globalTimer > 0f) globalTimer -= Time.deltaTime;

        if (!isRush)
        {
            rushCooldownTimer -= Time.deltaTime;
            if (rushCooldownTimer <= 0f)
            {
                if (Random.value < rushChance)
                {
                    StartRush();
                }
                else
                {
                    rushCooldownTimer = 5f;
                }
            }
        }
        else
        {
            rushTimer -= Time.deltaTime;
            if (rushTimer <= 0f) EndRush();
        }
    }

    public bool TryRequestAttack(EnemyAI requester)
    {
        //if (!isRush && globalTimer > 0f) return false;
        //if (!attackQueue.Contains(requester)) attackQueue.Enqueue(requester);
        //if (currentAttacking >= MaxAttacker) return false;
        //if (attackQueue.Peek() != requester) return false;

        //attackQueue.Dequeue();
        //currentAttacking++;
        //if (!isRush) globalTimer = globalCooldown;
        return true;
    }

    //攻撃終了
    public void EndAttack(EnemyAI enemy)
    {
        //isDashing = false;
        //attackTimer = 0f
        //dashTimer = 0f;

        currentAttacking = Mathf.Max(0, currentAttacking - 1);
    }

    private void StartRush()
    {
        //isRush = true;
        //rushTimer = rushDuration;
        //MaxAttacker = baseMaxAttacker * 2; //example: ラッシュで枠を増やす
        //globalTimer = 0f;
        //rushCooldownTimer = rushCooldown;
    }

    private void EndRush()
    {
        //isRush = false;
        //MaxAttacker = baseMaxAttacker;
    }

    public int CurrentAttacking => currentAttacking;
}