using UnityEngine;
using System.Threading;

public class RandomMove : MonoBehaviour
{
    [Header("移動開始するか")][SerializeField] bool IsMove;
    [Header("最大移動値")][SerializeField] float[] MaxMove = new float[3];
    [Header("最小,最大移動時間")][SerializeField] float[] MoveTimeLimit = new float[2];

    public struct MoveInfo
    {
        public float m_fInitPos;            //初期位置
        public float m_fCurrentMoveCount;   //移動経過時間
        public float m_fCurrentMove;        //現在の移動値
        public float m_fMoveSpeed;          //移動値
        public float m_fMoveTime;           //移動時間
        public float m_fStartPos;           //移動開始地点
        public float m_fGoalPos;            //移動終了地点
        public bool m_InMoving;             //移動と戻る
    }

    private MoveInfo[] m_MoveInfo=new MoveInfo[3];    //移動情報

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期位置を設定
        m_MoveInfo[0].m_fInitPos = gameObject.transform.localPosition.x;
        m_MoveInfo[1].m_fInitPos = gameObject.transform.localPosition.y;
        m_MoveInfo[2].m_fInitPos = gameObject.transform.localPosition.z;

        for (int i = 0; i < 3; i++)
        {
            MoveInit(i);
            TimeInit(i);

            //初期値から移動
            m_MoveInfo[i].m_InMoving = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMove) return;

        for (int i = 0; i < 3; i++)
        {
            if (MaxMove[i] == 0f) continue;

            m_MoveInfo[i].m_fCurrentMoveCount += Time.deltaTime;
            //移動
            m_MoveInfo[i].m_fCurrentMove = Mathf.Lerp(m_MoveInfo[i].m_fStartPos, m_MoveInfo[i].m_fGoalPos, m_MoveInfo[i].m_fCurrentMoveCount / m_MoveInfo[i].m_fMoveTime);

            //移動が完了したら
            if (m_MoveInfo[i].m_fCurrentMove != m_MoveInfo[i].m_fGoalPos) continue;

            if (m_MoveInfo[i].m_InMoving)
            {
                //元の位置に戻す
                m_MoveInfo[i].m_fStartPos = m_MoveInfo[i].m_fGoalPos;
                m_MoveInfo[i].m_fGoalPos = m_MoveInfo[i].m_fInitPos;
                TimeInit(i);
                m_MoveInfo[i].m_InMoving = false;
            }
            else
            {
                //移動する
                MoveInit(i);
                TimeInit(i);
                m_MoveInfo[i].m_InMoving = true;
            }
        }

        //座標を適応
        Vector3 pos = new Vector3(m_MoveInfo[0].m_fCurrentMove, m_MoveInfo[1].m_fCurrentMove, m_MoveInfo[2].m_fCurrentMove);
        gameObject.transform.localPosition = pos;
    }

    private void MoveInit()
    {
        for (int i = 0; i < 3; i++)
        {
            //移動量を求める
            m_MoveInfo[i].m_fMoveSpeed = Random.Range(-MaxMove[i], MaxMove[i]);
                
            m_MoveInfo[i].m_fMoveSpeed = 0f;
        }

        //移動開始地点,終了地点を求める
        m_MoveInfo[0].m_fStartPos = gameObject.transform.localPosition.x;
        m_MoveInfo[1].m_fStartPos = gameObject.transform.localPosition.y;
        m_MoveInfo[2].m_fStartPos = gameObject.transform.localPosition.z;
        for (int i = 0; i < 3; i++)
        {
            m_MoveInfo[i].m_fGoalPos = m_MoveInfo[i].m_fStartPos + m_MoveInfo[i].m_fMoveSpeed; 
        }
    }
    private void MoveInit(int ID)
    {
        //移動量を求める
        m_MoveInfo[ID].m_fMoveSpeed = Random.Range(-MaxMove[ID], MaxMove[ID]);

        //移動開始地点,終了地点を求める
        switch(ID)
        {
            case 0:
                m_MoveInfo[0].m_fStartPos = gameObject.transform.localPosition.x;
                break;

            case 1:
                m_MoveInfo[1].m_fStartPos = gameObject.transform.localPosition.y;
                break;

            case 2:
                m_MoveInfo[2].m_fStartPos = gameObject.transform.localPosition.z;
                break;
        }

        m_MoveInfo[ID].m_fGoalPos = m_MoveInfo[ID].m_fStartPos + m_MoveInfo[ID].m_fMoveSpeed;
    }

    private void TimeInit()
    {
        for (int i = 0; i < 3; i++)
        {
            //移動時間を決定
            m_MoveInfo[i].m_fMoveTime = Random.Range(MoveTimeLimit[0], MoveTimeLimit[1]);

            //カウントの初期化
            m_MoveInfo[i].m_fCurrentMoveCount = 0f;
            m_MoveInfo[i].m_fCurrentMove = 0f;
        }
    }
    private void TimeInit(int ID)
    {
        //移動時間を決定
        m_MoveInfo[ID].m_fMoveTime = Random.Range(MoveTimeLimit[0], MoveTimeLimit[1]);

        //カウントの初期化
        m_MoveInfo[ID].m_fCurrentMoveCount = 0f;
    }
}
