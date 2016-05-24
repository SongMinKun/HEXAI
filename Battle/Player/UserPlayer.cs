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
                //return;
            }
        }

        if (act == ACT.IDLE)
        {
            //if (pm.Players[pm.CurTurnIdx] == this)                              //요기고침
            //{
            //    MapManager.GetInst().SetHexColor(CurHex, Color.gray);
            //}
            if (pm.CurPlayer == this)                              //요기고침
            {
                MapManager.GetInst().SetHexColor(CurHex, Color.gray);
            }
        }

        if (act == ACT.MOVING)
        {
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

    public void OnMouseDown()                                               //cluod 
    {
        PlayerManager pm = PlayerManager.GetInst();
        Debug.Log(this.status.Name);

        if( this.act == ACT.SELECT)
        {
            //pm.CurTurnIdx = pm.Players.IndexOf(this);                       //cluod 
            this.act = ACT.IDLE;
            pm.CurPlayer = this;
        }

        // todo : 선택되면 ACT.IDLE로 만들기
    }
}

