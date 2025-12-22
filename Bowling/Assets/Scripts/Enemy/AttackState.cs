using UnityEngine;
using UnityEngine.AI;
using static IState;

//攻撃状態
public class AttackState : IState
{
    public StateType Type => StateType.Attack;

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
    }

    public void OnUpdate()
    {
        var player = enemy.Player;
        //プレイヤーが空であれば処理しない
        if (player == null) return;
        //ノックバック中は処理しない
        if (enemy.IsKnockBack) return;

        //プレイヤー方向ベクトルと距離を計算
        Vector3 toPlayer = player.position - enemy.transform.position;
        float distance = toPlayer.magnitude;
        Vector3 toPlayerDir = toPlayer.normalized;

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

        Vector3 desiredPos = Vector3.zero;
        Vector3 angle = Vector3.zero;

        if (distance > enemy.KeepDistance)
        {
            //少し遠い → 接近
            desiredPos = toPlayerDir;
            angle = enemy.transform.forward;

            attackTimer = 0;
        }
        else if (distance < enemy.RetreatDistance && !isDashing)
        {
            //近すぎる → 後退を強化
            desiredPos = -toPlayerDir;
            angle = -enemy.transform.forward;

            attackTimer = 0;
        }
        else
        {
            //攻撃タイマー更新
            attackTimer += Time.deltaTime;
            //Debug.Log($"プレイヤーがちょうどいい距離にいます");
        }

        //Boids補正
        Vector3 boidsForce = enemy.Boids.GetBoidsForceOptimized() * 0.9f;
        //方向補正（急な方向転換を防ぐ）
        Vector3 moveDir = Vector3.Slerp(angle, (desiredPos + boidsForce).normalized, 0.4f);
        //NavMesh上の有効な地点を探して移動
        Vector3 targetPos = enemy.transform.position + moveDir * 2.0f;
        //3m先を目標にする
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(hit.position);
        }

        //攻撃条件（範囲内かつクールダウン経過
        if (attackTimer >= enemy.AttackInterval)
        {
            if (Time.time >= nextAttackRequestTime &&
            enemy.AttackCtrl.TryRequestAttack(enemy))
            {
                nextAttackRequestTime = Time.time + attackRequestCooldown;
                if (!isDashing)
                {
                    //突進方向を決定（最初の1回だけ）
                    dashDir = (player.position - enemy.transform.position).normalized;
                    dashDir.y = 0f;
                    isDashing = true;
                    dashTimer = 0f;
                    Debug.Log($"突進攻撃を開始！");
                }
            }
        }
        else
        {
            //Debug.Log($"クールダウンが終わっていません: {attackTimer}");
        }

        PerformAttack();

        //距離が離れたら追跡へ戻る
        if (distance > 10f)
        {
            enemy.ChangeState(new ChaseState(enemy));
            Debug.Log($"プレイヤーが離れたため追跡へ戻る");
        }
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