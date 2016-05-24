using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager {

    private GameManager gm = null;
    private static PlayerManager inst = null;
    public GameObject GO_userPlayer; // unity에서 드래그로 적용한 프리팹
    public GameObject GO_aiPlayer; // unity에서 드래그로 적용한 프리팹

    public List<PlayerBase> Players = new List<PlayerBase>();           // 현재 여기의 정보로 턴을 판단해서 그 차례를 찾음
                                                                        // todo : 턴을 받아 오는 방식을 바꿀 필요가 있음

    public List<PlayerBase> User_Players = new List<PlayerBase>();      // 유저를 관리하는 리스트
    public List<PlayerBase> AI_Players = new List<PlayerBase>();        // ai 관리하는 리스트

    public PlayerBase CurPlayer;

    public int CurTurnIdx;//= 0;

    public int CurTurnCount = 0;                                        // 턴 카운트용 변수
    public int index = 0;

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
        //UserPlayer player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        //Hex hex = MapManager.GetInst().GetPlayerHex(-3, 3, 0);
        //hex.isExist = true;
        //player.CurHex = hex;
        //player.transform.position = player.CurHex.transform.position;
        //player.transform.Rotate(0, 90, 0);
        //Players.Add(player);
        //User_Players.Add(player);                               // user list에 추가 
        //GUIManager.GetInst().AddTurnPlayer(player);

        UserPlayer player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        Hex hex = MapManager.GetInst().GetPlayerHex(-3, 3, 0);
        hex.isExist = true;
        player.CurHex = hex;
        player.transform.position = player.CurHex.transform.position;
        player.transform.Rotate(0, 90, 0);
        player.act = ACT.SELECT;
        Players.Add(player);
        User_Players.Add(player);                               // user list에 추가 
        GUIManager.GetInst().AddTurnPlayer(player);

        player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        hex = MapManager.GetInst().GetPlayerHex(-2, 3, -1);
        hex.isExist = true;
        player.CurHex = hex;
        player.transform.position = player.CurHex.transform.position;
        player.transform.Rotate(0, 90, 0);
        player.act = ACT.SELECT;
        Players.Add(player);
        User_Players.Add(player);                               // user list에 추가 
        GUIManager.GetInst().AddTurnPlayer(player);

        player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        hex = MapManager.GetInst().GetPlayerHex(-1, 3, -2);
        hex.isExist = true;
        player.CurHex = hex;
        player.transform.position = player.CurHex.transform.position;
        player.transform.Rotate(0, 90, 0);
        player.act = ACT.SELECT;
        Players.Add(player);
        User_Players.Add(player);                               // user list에 추가 
        GUIManager.GetInst().AddTurnPlayer(player);

        player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        hex = MapManager.GetInst().GetPlayerHex(-3, 2, 1);
        hex.isExist = true;
        player.CurHex = hex;
        player.transform.position = player.CurHex.transform.position;
        player.transform.Rotate(0, 90, 0);
        player.act = ACT.SELECT;
        Players.Add(player);
        User_Players.Add(player);                               // user list에 추가 
        GUIManager.GetInst().AddTurnPlayer(player);

        player = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        hex = MapManager.GetInst().GetPlayerHex(-3, 1, 2);
        hex.isExist = true;
        player.CurHex = hex;
        player.transform.position = player.CurHex.transform.position;
        player.transform.Rotate(0, 90, 0);
        player.act = ACT.SELECT;
        Players.Add(player);
        User_Players.Add(player);                               // user list에 추가 
        GUIManager.GetInst().AddTurnPlayer(player);

        AIPlayer player2 = ((GameObject)GameObject.Instantiate(GO_aiPlayer)).GetComponent<AIPlayer>();
        Hex hex2 = MapManager.GetInst().GetPlayerHex(3, -3, 0);
        hex2.isExist = true;
        player2.CurHex = hex2;
        player2.transform.position = player2.CurHex.transform.position;
        player2.transform.Rotate(0, -90, 0);
        Players.Add(player2);
        AI_Players.Add(player2);                                // ai list에 추가
        GUIManager.GetInst().AddTurnPlayer(player2);

        /*player2 = ((GameObject)GameObject.Instantiate(GO_aiPlayer)).GetComponent<AIPlayer>();
        hex2 = MapManager.GetInst().GetPlayerHex(3, -2, -1);
        hex2.isExist = true;
        player2.CurHex = hex2;
        player2.transform.position = player2.CurHex.transform.position;
        player2.transform.Rotate(0, -90, 0);
        Players.Add(player2);
        AI_Players.Add(player2);                                // ai list에 추가
        GUIManager.GetInst().AddTurnPlayer(player2);

        player2 = ((GameObject)GameObject.Instantiate(GO_aiPlayer)).GetComponent<AIPlayer>();
        hex2 = MapManager.GetInst().GetPlayerHex(3, -1, -2);
        hex2.isExist = true;
        player2.CurHex = hex2;
        player2.transform.position = player2.CurHex.transform.position;
        player2.transform.Rotate(0, -90, 0);
        Players.Add(player2);
        AI_Players.Add(player2);                                // ai list에 추가
        GUIManager.GetInst().AddTurnPlayer(player2);

        player2 = ((GameObject)GameObject.Instantiate(GO_aiPlayer)).GetComponent<AIPlayer>();
        hex2 = MapManager.GetInst().GetPlayerHex(2, -3, 1);
        hex2.isExist = true;
        player2.CurHex = hex2;
        player2.transform.position = player2.CurHex.transform.position;
        player2.transform.Rotate(0, -90, 0);
        Players.Add(player2);
        AI_Players.Add(player2);                                // ai list에 추가
        GUIManager.GetInst().AddTurnPlayer(player2);

        player2 = ((GameObject)GameObject.Instantiate(GO_aiPlayer)).GetComponent<AIPlayer>();
        hex2 = MapManager.GetInst().GetPlayerHex(1, -3, 2);
        hex2.isExist = true;
        player2.CurHex = hex2;
        player2.transform.position = player2.CurHex.transform.position;
        player2.transform.Rotate(0, -90, 0);
        Players.Add(player2);
        AI_Players.Add(player2);                                // ai list에 추가
        GUIManager.GetInst().AddTurnPlayer(player2);*/
    }

    // 완료
    // 이동 구현
    public void MovePlayer(Hex start, Hex dest)
    {
        PlayerBase pb = CurPlayer;//Players[CurTurnIdx];

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

                start.isExist = false;
                dest.isExist = true;

                if (pb.MoveHexes.Count == 0)
                {
                    return;
                }

                pb.act = ACT.MOVING;

                MapManager.GetInst().ResetMapColor();

                // 이동 된 후에 자기가 있었던 Hex에서 isExist를 false로
                // todo : 상대가 있는 곳도 이동이 안되게
            }
        }
    }

    public void SetTurnOverTime(float time)
    {
        turnOverTime = time;
        curTurnOverTime = Time.smoothDeltaTime;
    }

    // todo : 마우스 다운으로 선택한 캐릭터 받아오기
    // 플레이어 선택하기
    public PlayerBase ChoosePlayer()
    {


        return null;
    }
    
    // 유저는 선택해서
    // ai는 랜덤으로?? 
    // 현재 플레이어와 ai 리스트를 따로 관리
    public void TurnOver()
    {
        MapManager.GetInst().ResetMapColor();
        PlayerBase pb = CurPlayer;// Players[CurTurnIdx];        // 마지막으로 실행 되었던 플레이어'
        PlayerBase up = User_Players[0];
        PlayerBase ap = AI_Players[index];

        if (pb is AIPlayer)
        {
            Debug.Log("ai 턴 끝남 " + pb.act);
            pb.act = ACT.IDLE;
            //CurTurnIdx = Players.IndexOf(User_Players[0]);          //cluod 
            CurPlayer = up;
            while( index+1 < AI_Players.Count && AI_Players[index+1] == null)
            {
                index++;
            }
            index++;
        }
        
        if (pb is UserPlayer)                                       //cloud
        {          
            pb.act = ACT.SELECT;                                    //cluod 
            //CurTurnIdx = Players.IndexOf(AI_Players[0]);            //cluod 
            CurPlayer = ap;
        }

        //if (CurTurnIdx >= Players.Count)
        //{
        //    CurTurnIdx = 0;
        //}

        if( index >= AI_Players.Count)
        {
            index = 0;
        }
        ////////////////////////////////////////////////////////

        //if( (CurTurnCount%2) == 0 )
        //{
        //    pb = up;
        //}

        ////////////////////////////////////////////////////////

        //CurTurnIdx++;                                             //cluod 

        GameObject.Destroy(EffectManager.GetInst().go);

        CurTurnCount++;
        Debug.Log("현재 " + CurTurnCount + "턴 째 진행완료");

    }

    int check;
    public void RemovePlayer(PlayerBase pb)
    {
        
        // ai 플레이어일 경우 순서를 바꾼다.
        if( pb is AIPlayer)
        {
            //check = AI_Players.IndexOf(pb);
            //PlayerBase temp = AI_Players[check];
            //AI_Players[check] = AI_Players[AI_Players.Count - 1];
            // pb >> A , AI_Players[check] >> B, temp > t

            //check = AI_Players.Count - 1;
            //PlayerBase temp = pb;
            //pb = AI_Players[check];
            //AI_Players[check] = temp;

            //int pos = AI_Players.IndexOf(AI_Players[check]);
            //AI_Players.Remove(AI_Players[check]);
            ////GUIManager.GetInst().RemoveTurnPlayer(AI_Players[check], pos);
            //GameObject.Destroy(AI_Players[check].gameObject);

            int pos = Players.IndexOf(pb);
            Players.Remove(pb);
            GameObject.Destroy(pb.gameObject);
        }
        if (pb is UserPlayer)
        {
            int pos = Players.IndexOf(pb);
            Players.Remove(pb);
            //GUIManager.GetInst().RemoveTurnPlayer(pb, pos);
            GameObject.Destroy(pb.gameObject);
        }

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
            PlayerBase pb = CurPlayer;//Players[CurTurnIdx];

            if (pb is AIPlayer)
            {
                return;
            }

            //ACT act = Players[CurTurnIdx].act;
            ACT act = CurPlayer.act;

            if( act == ACT.SELECT)
            {
                return;
            }

            // 1. idle 상태일 때는 할 일이 없음
            if (act == ACT.IDLE)
            {
                //Players[CurTurnIdx].act = ACT.SELECT;             // cloud
                CurPlayer.act = ACT.SELECT;
                MapManager.GetInst().ResetMapColor();               // cloud
            }

            // 2. move나 attack 상태일 때는 하이라이트를 초기화하고 idle 상태로 되돌림
            if ((act == ACT.MOVEHIGHLIGHT) || (act == ACT.ATTACKHIGHLIGHT))
            {
                //Players[CurTurnIdx].act = ACT.SELECT;
                CurPlayer.act = ACT.SELECT;
                MapManager.GetInst().ResetMapColor();
            }
        }
    }
}