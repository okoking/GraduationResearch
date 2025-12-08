using System.Collections.Generic;
using UnityEngine;

public class AttackController
{
    [Header("Å‘å“¯UŒ‚”")]
    [SerializeField] private int baseMaxAttacker = 3;

    public int MaxAttacker { get; private set; } = 3;
    private int currentAttacking = 0;
    private Queue<EnemyAI> attackQueue = new Queue<EnemyAI>();
    private float globalCooldown = 2.5f;
    private float globalTimer = 0f;

    //ƒ‰ƒbƒVƒ…
    private bool isRush = false;
    private float rushChance = 0.1f;
    private float rushDuration = 4f;
    private float rushTimer = 0f;
    private float rushCooldown = 10f;
    private float rushCooldownTimer = 0f;

    public AttackController()
    {
        MaxAttacker = baseMaxAttacker;
    }

    public void Update()
    {
        if (globalTimer > 0f) globalTimer -= Time.deltaTime;

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
        if (!isRush && globalTimer > 0f) return false;
        if (!attackQueue.Contains(requester)) attackQueue.Enqueue(requester);
        if (currentAttacking >= MaxAttacker) return false;
        if (attackQueue.Peek() != requester) return false;

        attackQueue.Dequeue();
        currentAttacking++;
        if (!isRush) globalTimer = globalCooldown;
        return true;
    }

    //UŒ‚I—¹
    public void EndAttack(EnemyAI enemy)
    {
        //isDashing = false;
        //attackTimer = 0f
        //dashTimer = 0f;

        currentAttacking = Mathf.Max(0, currentAttacking - 1);
    }

    private void StartRush()
    {
        isRush = true;
        rushTimer = rushDuration;
        MaxAttacker = baseMaxAttacker * 2; //example: ƒ‰ƒbƒVƒ…‚Å˜g‚ğ‘‚â‚·
        globalTimer = 0f;
        rushCooldownTimer = rushCooldown;
    }

    private void EndRush()
    {
        isRush = false;
        MaxAttacker = baseMaxAttacker;
    }

    public int CurrentAttacking => currentAttacking;
}