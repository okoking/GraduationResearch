
public enum StateType
{
    Patrol,
    Chase,
    Attack,
    Idle
}

//インターフェース(状態クラスの共通処理)
public interface IState
{
    public StateType Type { get; }

    //初期化処理
    void OnStart();
    //更新処理
    void OnUpdate();
    //終了処理
    void OnExit();
}