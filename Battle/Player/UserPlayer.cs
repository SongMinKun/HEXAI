using UnityEngine;
using System.Collections;

// 완료
public class UserPlayer : PlayerBase {
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
                pm.RemovePlayer(this);
                pm.TurnOver();
                return;
            }
        }

        if (act == ACT.IDLE)
        {
            if (pm.Players[pm.CurTurnIdx] == this)
            {
                MapManager.GetInst().SetHexColor(CurHex, Color.gray);
            }
        }

        if (act == ACT.MOVING)
        {
            //anim.SetBool("Attack", false);
            anim.SetBool("Run", true);

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
                    act = ACT.IDLE;
                    CurHex = nextHex;
                    anim.SetBool("Run", false);
                    
                    PlayerManager.GetInst().TurnOver();
                }
            }
        }
    }

    /*
    public override void DrawStatus()
    {
        float btnW = 100f;
        float btnH = 50f;

        Rect rect = new Rect(0, (Screen.height / 2) - btnH * 4, btnW, btnH);
        GUI.Label(rect, "Name : " + status.Name);

        rect = new Rect(0, (Screen.height / 2) - btnH * 3, btnW, btnH);
        GUI.Label(rect, "HP : " + status.CurHp);

        rect = new Rect(0, (Screen.height / 2) - btnH * 2, btnW, btnH);
        GUI.Label(rect, "MoveRange : " + status.MoveRange);

        rect = new Rect(0, (Screen.height / 2) - btnH * 1, btnW, btnH);
        GUI.Label(rect, "AtkRange : " + status.AtkRange);

        base.DrawStatus();
    }

    // 완료
    // 커멘드 버튼 그림
    public override void DrawCommand()
    {
        float btnW = 100f;
        float btnH = 50f;

        // 버튼
        // 시작 x좌표, 시작 y좌표, 가로 길이, 세로 길이
        Rect rect = new Rect(0, Screen.height / 2, btnW, btnH);

        if (GUI.Button(rect, "Move"))
        {
            Debug.Log("Move");

            // 이동 경로만큼 Hilight
            if (MapManager.GetInst().HighLightMoveRange(CurHex, status.MoveRange) == true)
            {
                act = ACT.MOVEHIGHLIGHT;
            }
        }

        rect = new Rect(0, (Screen.height / 2) + btnH, btnW, btnH);

        if (GUI.Button(rect, "Attack"))
        {
            Debug.Log("Attack");

            // 이동 경로만큼 Hilight
            if (MapManager.GetInst().HighLightAtkRange(CurHex, status.AtkRange) == true)
            {
                act = ACT.ATTACKHIGHLIGHT;
            }
        }

        rect = new Rect(0, (Screen.height / 2) + (btnH * 2), btnW, btnH);

        if (GUI.Button(rect, "Turn Over"))
        {
            Debug.Log("Turn Over");

            PlayerManager.GetInst().TurnOver();
        }

        // 왜 쓰는건지?
        base.DrawCommand();
    }
     * */
}

