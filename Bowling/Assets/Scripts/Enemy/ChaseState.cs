using UnityEngine;
using UnityEngine.AI;

//追跡状態
public class ChaseState : IState
{
    public StateType Type => StateType.Chase;

    private EnemyAI enemy;
    private float encircleSign = 1f;

    public ChaseState(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public void OnStart()
    {
        enemy.anim.Play(AnimState.Move);
        //追跡方向の左右をランダム化
        encircleSign = Random.value > 0.5f ? 1f : -1f;
    }

    public void OnUpdate()
    {
        var player = enemy.Player;
        //プレイヤーが空であれば処理しない
        if (player == null) return;
        //ノックバック中は処理しない
        if (enemy.IsKnockBack) return;

        //プレイヤーを注視
        Vector3 lookPos = player.position;
        lookPos.y = enemy.transform.position.y;
        enemy.transform.LookAt(lookPos);

        //プレイヤー方向ベクトルと距離
        Vector3 toPlayer = player.position - enemy.transform.position;
        float distance = toPlayer.magnitude;
        Vector3 toPlayerDir = toPlayer.normalized;

        //左右方向
        Vector3 encircleDir = Quaternion.Euler(0, 90f * encircleSign, 0) * toPlayerDir;
        Vector3 desiredPos = Vector3.zero;

        //役割によって動きを変える
        if (enemy.Type == EnemyType.Melee)
        {
            if (distance > 8f)
            {
                switch (enemy.EnemyRole)
                {
                    case EnemyAI.Role.Front:
                        //前衛 → 直接突撃
                        desiredPos = toPlayerDir;
                        break;
                    case EnemyAI.Role.Side:
                        //側面 → 斜めに包囲
                        desiredPos = encircleDir * 0.8f + toPlayerDir * 0.3f; break;
                    case EnemyAI.Role.Back:
                        //後衛 → 少し距離をとって包囲
                        desiredPos = -toPlayerDir * 0.4f + encircleDir * 0.6f; break;
                }
            }
        }

        if (enemy.Type == EnemyType.Ranged)
        {
            // 近づきすぎたら離れる
            if (distance < enemy.IdealRangedDistance)
            {
                desiredPos = -toPlayerDir;
            }
            // 遠すぎたら近づく
            else if (distance > enemy.RangedAttackRange)
            {
                enemy.Agent.speed = 5.0f;
                desiredPos = toPlayerDir;
            }
            else
            {
                // 横移動（撃ちやすい位置取り）
                desiredPos = encircleDir;
            }
        }
        //Boids補正
        Vector3 boidsForce = enemy.Boids.GetBoidsForceOptimized() * 0.9f;
        //方向補正（急な方向転換を防ぐ）
        Vector3 side = Vector3.Cross(Vector3.up, toPlayerDir) * encircleSign;
        Vector3 spread = side * 0.3f;

        Vector3 moveDir = Vector3.Slerp(
            enemy.transform.forward,
            (desiredPos + boidsForce + spread).normalized,
            0.4f
        );
        //NavMesh上の有効な地点を探して移動
        Vector3 targetPos = enemy.transform.position + moveDir * 3.0f;
        
        //3m 先を目標にする
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(hit.position);
        }
        
        //離れすぎた場合は巡回状態に戻す
        if (distance > 20f)
        {
            enemy.ChangeState(new PatrolState(enemy));
            enemy.SetRandomPatrolPoint();
            Debug.Log("プレイヤーを見失い、巡回に戻る");
            //wasFar[0] = false;
            //wasFar[1] = false; return;
        }
       
        float attackdistance = 5f;
        //すでに攻撃態勢に入っている敵が４以上いた場合後方の敵も攻撃態勢に入る
        if (EnemyManager.Instance.IsEnemyAttacking() >= 4)
        { 
            attackdistance += 10f;
        }
        
        //攻撃・見失い処理（任意で再有効化）
        if (distance < attackdistance)
        {
            enemy.ChangeState(new AttackState(enemy));
            Debug.Log("攻撃状態へ");
            
        }
    }

    public void OnExit() { }
}