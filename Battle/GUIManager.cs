using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager {
    private static GUIManager inst = null;
    private PlayerManager pm = null;

    public static GUIManager GetInst()
    {
        if (inst == null)
        {
            inst = new GUIManager();
            inst.pm = PlayerManager.GetInst();
        }

        return inst;
    }

    // todo : 이 부분을 호출하는 것이 필요함
    public void DrawGUI()
    {
        if (pm.Players.Count > 0)
        {
            PlayerBase pb = pm.Players[pm.CurTurnIdx];

            if(pb is UserPlayer) {

                if (pb.act == ACT.IDLE)
                {
                    DrawStatus(pm.Players[pm.CurTurnIdx]);
                    DrawCommand(pm.Players[pm.CurTurnIdx]);
                }
            }
        }
        DrawTurnInfo();
    }

    // todo : 이 부분을 호출하는 것이 필요함
    public void DrawStatus(PlayerBase pb)
    {
        GUILayout.BeginArea(new Rect(0, Screen.height / 6 * 5, Screen.width / 3 , Screen.height / 6), "Player Info", GUI.skin.window);

        GUILayout.Label("Name : " + pb.status.Name);
        GUILayout.Label("HP : " + pb.status.CurHp);
        GUILayout.Label("Power : " + pb.status.Power);
        GUILayout.Label("MoveRange : " + pb.status.MoveRange);
        GUILayout.Label("AtkRange : " + pb.status.AtkRange);

        GUILayout.EndArea();
    }

    public void DrawTurnInfo()
    {

        GUILayout.BeginArea(new Rect(0, 0, Screen.width / 4, Screen.height / 8), "Turn Info", GUI.skin.window);

        if(pm.turnIdx % 2 == 0)
            GUILayout.Label(" Turn : " + ((pm.turnIdx / 2) + 1) + "  USER TURN");
        else
            GUILayout.Label(" Turn : " + ((pm.turnIdx / 2) + 1 )+ "  AI TURN");

        GUILayout.Label(" Turn : " + pm.turnIdx );


        GUILayout.EndArea();
    }

    // 완료
    // 커멘드 버튼 그림
    public void DrawCommand(PlayerBase pb)
    {
        GUILayout.BeginArea(new Rect(Screen.width / 3 * 2, Screen.height / 6 * 5, Screen.width / 3, Screen.height / 6), "Command", GUI.skin.window);
     
        // 버튼
        // 시작 x좌표, 시작 y좌표, 가로 길이, 세로 길이
        //Rect rect = new Rect(0, Screen.height / 2, btnW, btnH);

        if (GUILayout.Button("Move"))
        {
            Debug.Log("Move");

            // 이동 경로만큼 Hilight
            if (MapManager.GetInst().HighLightMoveRange(pb.CurHex, pb.status.MoveRange) == true)
            {
                pb.act = ACT.MOVEHIGHLIGHT;
            }
        }

        if (GUILayout.Button("Attack"))
        {
            Debug.Log("Attack");

            // 이동 경로만큼 Hilight
            if (MapManager.GetInst().HighLightAtkRange(pb.CurHex, pb.status.AtkRange) == true)
            {
                pb.act = ACT.ATTACKHIGHLIGHT;
            }
        }

        if (GUILayout.Button("Turn Over"))
        {
            Debug.Log("Turn Over");

            PlayerManager.GetInst().TurnOver();
        }

        GUILayout.EndArea();
    }
}
