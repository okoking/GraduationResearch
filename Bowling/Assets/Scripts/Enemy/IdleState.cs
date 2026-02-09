using UnityEngine;

//待機状態
public class IdleState : IState
{
    public StateType Type => StateType.Idle;

    private EnemyAI enemy;
    private float timer;
    private float waitTime;

    public IdleState(EnemyAI enemy)
    {
        this.enemy = enemy;
        waitTime = UnityEngine.Random.Range(1.5f, 2.0f);
    }

    //初期化処理
    public void OnStart()
    {
        enemy.anim.Play(AnimState.Idle);
        timer = 0f;
        //待ち時間再設定
        waitTime = Random.Range(1.0f, 2.5f);
    }

    //更新処理
    public void OnUpdate()
    {
        //視認でチェイス
        if (enemy.CanSeePlayer())
        {
            //追跡状態へ
            enemy.ChangeState(new ChaseState(enemy));
            //近くの敵へ警報
            enemy.Manager.AlertNearby(enemy, 5f);
            //ピックリマーク表示
            enemy.ShowAlert();
            return;
        }

        //待機タイマー更新
        timer += Time.deltaTime;

        //待ち時間が終わったら
        if (timer > waitTime)
        {
            ////ランダムパトロール地点設定
            //enemy.SetRandomPatrolPoint();
            //巡回状態へ変更
            enemy.ChangeState(new PatrolState(enemy));

            //var center = enemy.GetPatrolCenter();
            //Vector3 rnd = center + Random.insideUnitSphere * enemy.GetPatrolRadius();
            //rnd.y = enemy.transform.position.y;
            //if (UnityEngine.AI.NavMesh.SamplePosition(rnd, out var hit, enemy.GetPatrolRadius(), UnityEngine.AI.NavMesh.AllAreas))
            //{
            //    enemy.SetPatrolTarget(hit.position);
            //    enemy.ChangeState(new PatrolState(enemy));
            //}

            Debug.Log("待機状態の待ち時間が終わりました");
        }
    }

    //終了処理
    public void OnExit()
    {
        //Debug.Log("巡回状態へ");
    }
}