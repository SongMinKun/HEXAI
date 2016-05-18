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
    }

    // todo : 이 부분을 호출하는 것이 필요함
    public void DrawStatus(PlayerBase pb)
    {
        GUILayout.BeginArea(new Rect(0, Screen.height / 2, 150f, Screen.height / 2), "Player Info", GUI.skin.window);

        GUILayout.Label("Name : " + pb.status.Name);
        GUILayout.Label("HP : " + pb.status.CurHp);
        GUILayout.Label("MoveRange : " + pb.status.MoveRange);
        GUILayout.Label("AtkRange : " + pb.status.AtkRange);

        GUILayout.EndArea();
    }

    // 완료
    // 커멘드 버튼 그림
    public void DrawCommand(PlayerBase pb)
    {
        int cmdCnt = 3;
        float cmdW = 150f;
        float btnH = 50f;

        GUILayout.BeginArea(new Rect(Screen.width - cmdW, Screen.height - cmdCnt * btnH, cmdW, cmdCnt * btnH), "Command", GUI.skin.window);

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
