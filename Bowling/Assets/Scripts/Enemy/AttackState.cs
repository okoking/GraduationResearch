using UnityEngine;
using UnityEngine.AI;
using static IState;
enum AttackMoveMode
{
    None,
    Approach,
    Retreat
}

//攻撃状態
public class AttackState : IState
{
    public StateType Type => StateType.Attack;
    private AttackMoveMode moveMode = AttackMoveMode.None;

    private EnemyAI enemy;

    private float attackTimer = 0f;             //攻撃間隔
    private bool isDashing = false;             //突進するか
    private float dashTimer = 0f;               //タイマー
    private Vector3 dashDir;
    private float nextAttackRequestTime = 0f;
    public float attackRequestCooldown = 2f;    // 攻撃再申請までの秒数

    public AttackState(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public void OnStart()
    {
        attackTimer = 0f;
        isDashing = false;
        dashTimer = 0f;

        moveMode = AttackMoveMode.None;
    }

    public void OnUpdate()
    {
        var player = enemy.Player;
        //プレイヤーが空であれば処理しない
        if (player == null || enemy.IsKnockBack) return;

        //プレイヤー方向ベクトルと距離を計算
        Vector3 toPlayer = player.position - enemy.transform.position;
        float distance = toPlayer.magnitude;

        //回転は滑らかに
        if (!isDashing)
        {
            //プレイヤーを注視
            //プレイヤー方向のベクトル
            Vector3 dir = player.position - enemy.transform.position;

            //水平だけ向く
            dir.y = 0;

            if (dir.sqrMagnitude > 0.001f)
            {
                //目標回転
                Quaternion target = Quaternion.LookRotation(dir);

                //徐々に回転する
                float rotateSpeed = 5f;

                //回転の速さ
                enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation, target, Time.deltaTime * rotateSpeed);
            }
        }

        if (enemy.Type == EnemyType.Melee)
        {
            UpdateMelee(player, distance, toPlayer.normalized);
        }
        else
        {
            UpdateRanged(player, distance, toPlayer.normalized);
        }
    }
    void UpdateMelee(Transform player, float distance, Vector3 toPlayerDir)
    {
        enemy.Agent.speed = 5f;

        //モード決定（毎フレーム SetDestination しない）
        if (!isDashing)
        {
            if (distance > enemy.KeepDistance + 0.5f)
            {
                Debug.Log("遠すぎるので近づきます");
                SetMoveMode(AttackMoveMode.Approach, toPlayerDir);
            }
            else if (distance < enemy.RetreatDistance - 0.5f)
            {
                Debug.Log("近すぎるので離れます");
                SetMoveMode(AttackMoveMode.Retreat, -toPlayerDir);
            }
            else
            {
                StopMove();
                attackTimer += Time.deltaTime;
                Debug.Log("ちょうどいい距離にいます");
            }
        }

        // 攻撃開始
        if (attackTimer >= enemy.AttackInterval &&
            Time.time >= nextAttackRequestTime &&
            enemy.AttackCtrl.TryRequestAttack(enemy))
        {
            nextAttackRequestTime = Time.time + attackRequestCooldown;

            if (!isDashing)
            {
                dashDir = (player.position - enemy.transform.position).normalized;
                dashDir.y = 0f;
                isDashing = true;
                dashTimer = 0f;
            }
        }
        PerformAttack();
    }

    void UpdateRanged(Transform player, float distance, Vector3 toPlayerDir)
    {
        enemy.Agent.speed = 5f;

        float keepdis = enemy.KeepDistance + 5.0f;
        float retdis = enemy.RetreatDistance + 5.0f;

        //モード決定（毎フレーム SetDestination しない）
        if (!isDashing)
        {
            if (distance > keepdis + 0.5f)
            {
                Debug.Log("遠すぎるので近づきます");
                SetMoveMode(AttackMoveMode.Approach, toPlayerDir);
            }
            else if (distance < retdis - 0.5f)
            {
                Debug.Log("近すぎるので離れます");
                SetMoveMode(AttackMoveMode.Retreat, -toPlayerDir);
            }
            else
            {
                StopMove();
                attackTimer += Time.deltaTime;
                Debug.Log("ちょうどいい距離にいます");
            }
        }

        if (attackTimer >= enemy.AttackInterval &&
           Time.time >= nextAttackRequestTime &&
           enemy.AttackCtrl.TryRequestAttack(enemy))
        {
            nextAttackRequestTime = Time.time + attackRequestCooldown;

            enemy.FireMissile();
            attackTimer = 0f;
            
        }
    }

    void SetMoveMode(AttackMoveMode mode, Vector3 dir)
    {
        ////if (moveMode == mode) return;

        moveMode = mode;

        Vector3 targetPos = enemy.transform.position + dir.normalized * 2.0f;
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(hit.position);
        }
    }

    void StopMove()
    {
        if (moveMode == AttackMoveMode.None) return;

        moveMode = AttackMoveMode.None;
        enemy.Agent.ResetPath();
    }

    //攻撃処理
    public void PerformAttack()
    {
        if (!isDashing) return;

        //突進移動
        dashTimer += Time.deltaTime;
        enemy.transform.position += dashDir * enemy.DashSpeed * Time.deltaTime;

        //衝突判定
        Collider[] hits = Physics.OverlapSphere(enemy.transform.position
        + enemy.transform.forward * 0.5f, enemy.AttackRadius);

        foreach (var h in hits)
        {
            if (h.CompareTag("Player"))
            {
                Debug.Log($"の突進がプレイヤーに命中！");

                h.GetComponent<PlayerHealth>()?.TakeDamage(enemy.AttackPower);

                EndAttack();
                return;
            }

            //時間で突進終了
            if (dashTimer > enemy.DashTime)
            {
                EndAttack();
            }
        }
    }

    //攻撃終了
    void EndAttack()
    {
        isDashing = false;
        attackTimer = 0f;
        dashTimer = 0f;
        enemy.AttackCtrl.EndAttack(enemy);
    }

    private void EndDash()
    {
        isDashing = false;
        enemy.AttackCtrl.EndAttack(enemy);
    }

    public void OnExit()
    {
        //確実に攻撃枠を開放
        enemy.AttackCtrl.EndAttack(enemy);
    }
}