using UnityEngine;
using System.Collections;

public class AIPlayer : PlayerBase {

    void Awake()
    {
        act = ACT.IDLE;
        anim = GetComponent<Animator>();
        status = new PlayerStatus();
    }

    // Use this for initialization
    void Start()
    {

    }

    // 완료
    // Update is called once per frame
    // 플레이어의 이동시 모습을 스무스하게 구현
    void Update()
    {
        PlayerManager pm = PlayerManager.GetInst();

        if (removeTime != 0)
        {
            removeTime += Time.deltaTime;
            if (removeTime >= 3.3f)
            {
                pm.TurnOver();
                pm.RemovePlayer(this);
            }
        }

        if (act == ACT.IDLE) // idle 상태일 때 무엇인가를 한다
        {
            if (pm.Players[pm.CurTurnIdx] == this)
            {
                MapManager.GetInst().SetHexColor(CurHex, Color.gray);

                AIProc();
            }
        }

        else if (act == ACT.MOVING)
        {
            anim.SetBool("Run", true);

            // 이미 목적지에 도착한 상태
            if (MoveHexes.Count == 0)
            {
                act = ACT.IDLE;

                PlayerManager.GetInst().TurnOver();

                return;
            }

            Hex nextHex = MoveHexes[0];

            float distance = Vector3.Distance(transform.position, nextHex.transform.position);

            if (distance > 0.1f) // 아직 이동 해야함
            {
                transform.position += (nextHex.transform.position - transform.position).normalized * status.MoveSpeed * Time.smoothDeltaTime;

                transform.rotation = Quaternion.LookRotation((nextHex.transform.position - transform.position).normalized);
            }

            else // 다음 목표 Hex에 도착함
            {
                transform.position = nextHex.transform.position;
                MoveHexes.RemoveAt(0);

                // 모든 Hex가 remove가 되면 최종 dest에 도착
                if (MoveHexes.Count == 0)
                {
                    anim.SetBool("Run", false);

                    // 이동할 때 원래 있던 자리의 isExit은 false로 도착한 자리의 isExist은 true로 ( SMK )
                    CurHex.isExist = false;
                    nextHex.isExist = true;


                    act = ACT.IDLE;
                    CurHex = nextHex;

                    PlayerManager.GetInst().TurnOver();
                }
            }
        }
    }

    // todo : AI 구동 원리
    public void AIProc()
    {
        AI ai = AI.GetInst();

        // 1. 근접한 플레이어를 찾아 인접한 셀까지 이동
        // 만약 이미 근접한 상태이면 act는 IDLE를 유지한다. 이동이 필요하면 act는 MOVING로 바뀐다
        ai.MoveAIToNearUserPlayer(this);

        // 2. 는 1.에서 이동이 필요 없는 상태로 즉, 이미 근접한 상태일 때는 공격을 시도한다
        if (act == ACT.IDLE)
        {
            ai.AtkAItoUser(this);
        }

        // 이미 목적지에 도착한 상태로 공격을 시도
        if (MoveHexes.Count == 0)
        {

        }
    }

    void OnMouseDown()
    {
        PlayerManager pm = PlayerManager.GetInst();
        PlayerBase pb = pm.Players[pm.CurTurnIdx];

        // 바닥을 누르면 체력이 달지 않고 상대방 말을 눌러야 체력이 단다
        // todo : 바닥 눌러도 달게
        if (pb.act == ACT.ATTACKHIGHLIGHT)
        {
            BattleManager.GetInst().AttackAtoB(pb, this);
        }
    }
}

