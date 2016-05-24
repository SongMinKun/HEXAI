using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager {

    private GameManager gm = null;
    private static PlayerManager inst = null;
    public GameObject GO_userPlayer; // unity에서 드래그로 적용한 프리팹
    public GameObject GO_aiPlayer; // unity에서 드래그로 적용한 프리팹

    public List<PlayerBase> Players = new List<PlayerBase>();

    public int CurTurnIdx = 0;
    public int turnIdx = 0;

    private float turnOverTime = 0;
    private float curTurnOverTime = 0;

    public static PlayerManager GetInst()
    {
        if (inst == null)
        {
            inst = new PlayerManager();
            inst.Inst();
        }

        return inst;
    }

    public void Inst()
    {
        turnOverTime = 0;
        curTurnOverTime = 0;

        GO_userPlayer = (GameObject)Resources.Load("Prefabs/Players/UserPlayer_King");
        GO_aiPlayer = (GameObject)Resources.Load("Prefabs/Players/AI_Skeleton");
    }

    public void CheckTurnOver()
    {
        if (curTurnOverTime != 0)
        {
            curTurnOverTime += Time.deltaTime;

            if (curTurnOverTime >= turnOverTime)
            {
                curTurnOverTime = 0f;
                TurnOver();
            }
        }
    }

    // 완료
    // 플레이어 선언 및 위치 배정
    public void GenPlayerTest()
    {
        UserPlayer player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        Hex hex = MapManager.GetInst().GetPlayerHex(-4, 4, 0);
        hex.isExist = true;
        player.CurHex = hex;
        player.transform.position = player.CurHex.transform.position;
        player.transform.Rotate(0, 90, 0);
        player.status = new PlayerStatus(UNIT.KING);    // 킹
        Players.Add(player);

        player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        hex = MapManager.GetInst().GetPlayerHex(-3, 2, 1);
        hex.isExist = true;
        player.CurHex = hex;
        player.transform.position = player.CurHex.transform.position;
        player.transform.Rotate(0, 90, 0);
        player.status = new PlayerStatus(UNIT.ARCHER);    // 아처
        Players.Add(player);
        
        AIPlayer player2 = ((GameObject)GameObject.Instantiate(GO_aiPlayer)).GetComponent<AIPlayer>();
        Hex hex2 = MapManager.GetInst().GetPlayerHex(3, -3, 0);
        hex2.isExist = true;
        player2.CurHex = hex2;
        player2.transform.position = player2.CurHex.transform.position;
        player2.transform.Rotate(0, -90, 0);
        player2.status = new PlayerStatus(UNIT.KING);    // 킹
        Players.Add(player2);

        player2 = ((GameObject)GameObject.Instantiate(GO_aiPlayer)).GetComponent<AIPlayer>();
        hex2 = MapManager.GetInst().GetPlayerHex(2, -3, 1);
        hex2.isExist = true;
        player2.CurHex = hex2;
        player2.transform.position = player2.CurHex.transform.position;
        player2.transform.Rotate(0, -90, 0);
        player2.status = new PlayerStatus(UNIT.TANKER);    // 탱커
        Players.Add(player2);

    }

    // 완료
    // 이동 구현
    public void MovePlayer(Hex start, Hex dest)
    {
        PlayerBase pb = Players[CurTurnIdx];

        if (MapManager.GetInst().isReachAble(start, dest, pb.status.MoveRange) == false)
        {
            return;
        }

        if (pb.act == ACT.MOVEHIGHLIGHT)
        {
            int distance = MapManager.GetInst().GetDistance(start, dest);

            if (distance <= pb.status.MoveRange && distance != 0 && dest.Passable == true && dest.isExist == false)
            {
                pb.MoveHexes = MapManager.GetInst().GetPath(start, dest);

                // 이동할 때 원래 있던 자리의 isExit은 false로 도착한 자리의 isExist은 true로 ( SMK )
                start.isExist = false;
                dest.isExist = true;

                if (pb.MoveHexes.Count == 0)
                {
                    return;
                }

                pb.act = ACT.MOVING;

                MapManager.GetInst().ResetMapColor();
            }
        }
    }

    public void SetTurnOverTime(float time)
    {
        turnOverTime = time;
        curTurnOverTime = Time.smoothDeltaTime;
    }

    // 완료
    // 턴 넘기면
    public void TurnOver()
    {
        MapManager.GetInst().ResetMapColor();
        TimerManager tm = TimerManager.GetInst();
        PlayerBase pb = Players[CurTurnIdx];

        pb.act = ACT.IDLE;

        turnIdx++;
        CurTurnIdx++;

        tm.resetTimer();

        if (CurTurnIdx >= Players.Count)
        {
            CurTurnIdx = 0;
        }

        GameObject.Destroy(EffectManager.GetInst().go);
    }

    public void RemovePlayer(PlayerBase pb)
    {
        //int pos = Players.IndexOf(pb);
        pb.CurHex.isExist = false;
        Players.Remove(pb);
        GameObject.Destroy(pb.gameObject);

        int enemyCnt = 0;
        int userCnt = 0;

        foreach (PlayerBase pb2 in Players)
        {
            if (pb2 is AIPlayer)
            {
                enemyCnt++;
            }

            else if(pb2 is UserPlayer)
            {
                userCnt++;
            }
        }

        if (enemyCnt == 0)
        {
            EventManager.GetInst().GameEnd = true;
            GameManager.GetInst().ShowStageClear();
        }

        else if (userCnt == 0)
        {
            EventManager.GetInst().GameEnd = true;
            GameManager.GetInst().ShowGameOver();
        }
    }

    public void MouseInputProc(int btn)
    {
        // 버튼 1(마우스 우클릭)이면 취소
        if (btn == 1)
        {
            // AI 플레이어일 경우에는 리턴
            PlayerBase pb = Players[CurTurnIdx];

            if (pb is AIPlayer)
            {
                return;
            }

            ACT act = Players[CurTurnIdx].act;

            // 1. idle 상태일 때는 할 일이 없음
            if (act == ACT.IDLE)
            {
                return;
            }

            // 2. move나 attack 상태일 때는 하이라이트를 초기화하고 idle 상태로 되돌림
            if ((act == ACT.MOVEHIGHLIGHT) || (act == ACT.ATTACKHIGHLIGHT))
            {
                Players[CurTurnIdx].act = ACT.IDLE;
                MapManager.GetInst().ResetMapColor();
            }
        }
    }
}