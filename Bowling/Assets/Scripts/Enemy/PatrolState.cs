using UnityEngine;
using UnityEngine.AI;

//巡回状態
public class PatrolState : IState
{
    //状態を設定
    public StateType Type => StateType.Patrol;

    private EnemyAI enemy;
    public PatrolState(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public void OnStart() { }

    public void OnUpdate()
    {
        //エージェントを取得
        var agent = enemy.Agent;

        //NavMesh上にいなければ何もしない
        if (!agent.enabled || !agent.isOnNavMesh)
        {
            return;
        }

        //プレイヤーを発見したら
        if (enemy.CanSeePlayer())
        {
            //追跡状態へ
            enemy.ChangeState(new ChaseState(enemy));
            //近くの敵に警告
            enemy.Manager.AlertNearby(enemy, 5f);
            //ビックリマーク表示
            enemy.ShowAlert();
            return;
        }

        //経路計算中は待機
        if (agent.pathPending) return;

        //目的地に到達したら
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //待機状態へ
            enemy.ChangeState(new IdleState(enemy));
            Debug.Log("待機状態へ");
            return;
        }

        //目的地
        Vector3 patrolTarget = enemy.GetPatrolTarget();
        //Boids補正で目的地微調整
        var boids = enemy.Boids.GetBoidsForceOptimized() * 0.9f;

        //NavMeshAgentが目指す目標方向を補正
        Vector3 targetDir = (patrolTarget - enemy.transform.position).normalized;
        Vector3 adjustedTarget = enemy.transform.position + 
            (targetDir + boids).normalized * 2f;

        //次の目標位置をBoids補正で微調整
        Vector3 direction = (patrolTarget - enemy.transform.position).normalized +
            boids;
        Vector3 adjustedPos = enemy.transform.position + direction.normalized * 2f;

        if (NavMesh.SamplePosition(adjustedPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void OnExit()
    {

    }

    //巡回範囲と目的地をGizmosで可視化
    void OnDrawGizmosSelected()
    {
        //色指定
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(enemy.GetPatrolCenter(), enemy.GetPatrolRadius());
        //巡回エリア
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, enemy.GetPatrolRadius());
        //1回分のランダム半径
        if (enemy.GetPatrolTarget() != Vector3.zero)
        {
            Gizmos.color = Color.red; Gizmos.DrawSphere(enemy.GetPatrolTarget(), 0.3f);
            Gizmos.DrawLine(enemy.transform.position, enemy.GetPatrolTarget());
        }
    }

    //敵の視界を可視化
    void OnDrawGizmos()
    {
        //視界距離（CanSeePlayerの距離と合わせる）
        float viewDistance = 10f;
        //視界角（CanSeePlayerの角度と合わせる）
        float viewAngle = 60f;
        //敵の向き 
        Vector3 forward = enemy.transform.forward;
        //扇形を描画
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        //半透明黄色
        DrawViewGizmo(enemy.transform.position + Vector3.up * 0.5f, forward, viewAngle, viewDistance);
        //レイキャスト方向の可視化
        Debug.DrawRay(enemy.transform.position + Vector3.up * 0.5f, forward * viewDistance, Color.yellow);
        
        //実際にプレイヤーが視界内にいる場合、赤い線を引く
        if (enemy.Player != null)
        {
            Vector3 dirToPlayer = (enemy.Player.position - enemy.transform.position).normalized;
            float distance = Vector3.Distance(enemy.transform.position, enemy.Player.position);
            float angle = Vector3.Angle(forward, dirToPlayer);
            if (distance < viewDistance && angle < viewAngle)
            {
                Debug.DrawLine(enemy.transform.position + Vector3.up, enemy.Player.position, Color.red);
            }
        }
    }

    //扇形を描画する補助関数
    void DrawViewGizmo(Vector3 position, Vector3 forward, float angle, float distance)
    {
        int segmentCount = 20;
        //扇形の分割数
        float step = angle * 2 / segmentCount;
        Vector3 oldPoint = position;
        for (int i = 0; i <= segmentCount; i++)
        {
            float currentAngle = -angle + step * i;
            Quaternion rot = Quaternion.Euler(0, currentAngle, 0);
            Vector3 dir = rot * forward;
            Vector3 nextPoint = position + dir * distance;
            if (i > 0) Gizmos.DrawLine(oldPoint, nextPoint);
            oldPoint = nextPoint;
        }
        //扇形の外周線
        Gizmos.DrawLine(position, position + Quaternion.Euler(0, -angle, 0) * forward * distance);
        Gizmos.DrawLine(position, position + Quaternion.Euler(0, angle, 0) * forward * distance);
    }
}