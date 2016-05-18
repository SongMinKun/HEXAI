using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI {
    private static AI inst = null;

    public static AI GetInst()
    {
        if (inst == null) { 
            inst = new AI();
        }

        return inst;
    }

    public void MoveAIToNearUserPlayer(PlayerBase aiPlayer)
    {
        PlayerManager pm = PlayerManager.GetInst();
        MapManager mm = MapManager.GetInst();

        PlayerBase nearUserPlayer = null;
        int nearDistance = 1000;
        
        // 1. 근접한 플레이어를 찾는다
        foreach (PlayerBase up in pm.Players)
        {
            if (up is UserPlayer)
            {
                int distance = mm.GetDistance(up.CurHex, aiPlayer.CurHex);

                if(nearDistance > distance) {
                    nearUserPlayer = up;
                    nearDistance = distance;
                }
            }
        }

        if (nearUserPlayer != null)
        {
            // 2. 근접한 플레이어로 이동한다
            List<Hex> path = mm.GetPath(aiPlayer.CurHex, nearUserPlayer.CurHex);

            // 이동 범위를 넘는지 체크
            if (path.Count > aiPlayer.status.MoveRange)
            {
                // ex 길이 10, 이동범위 3
                path.RemoveRange(aiPlayer.status.MoveRange, path.Count - aiPlayer.status.MoveRange);
            }

            aiPlayer.MoveHexes = path;

            if (nearUserPlayer.CurHex.MapPos == aiPlayer.MoveHexes[aiPlayer.MoveHexes.Count - 1].MapPos)
            {
                aiPlayer.MoveHexes.RemoveAt(aiPlayer.MoveHexes.Count - 1);
            }

            if (aiPlayer.MoveHexes.Count == 0)
            {
                return;
            }

            aiPlayer.act = ACT.MOVING;
            MapManager.GetInst().ResetMapColor(aiPlayer.CurHex.MapPos);
        }
        
        // 3. 만약 근접후 공격이 가능하면 공격한다
    }

    public void AtkAItoUser(PlayerBase aiPlayer)
    {
        PlayerManager pm = PlayerManager.GetInst();
        MapManager mm = MapManager.GetInst();

        PlayerBase nearUserPlayer = null;
        int nearDistance = 1000;

        // 1. 근접한 유저플레이어를 찾는다
        foreach (PlayerBase up in pm.Players)
        {
            if (up is UserPlayer)
            {
                int distance = mm.GetDistance(up.CurHex, aiPlayer.CurHex);

                if (nearDistance > distance)
                {
                    nearUserPlayer = up;
                    nearDistance = distance;
                }
            }
        }

        // 2-1. 찾으면 공격을 한다
        if (nearUserPlayer != null)
        {
            BattleManager.GetInst().AttackAtoB(aiPlayer, nearUserPlayer);
            //aiPlayer.transform.rotation = Quaternion.LookRotation((nearUserPlayer.CurHex.transform.position - aiPlayer.transform.position).normalized);
            // nearUserPlayer.GetDamage(10);
            //Debug.Log("AIplayer Attack!!");
            //aiPlayer.anim.SetBool("Attack", true);

            //BattleManager.GetInst().AttackAtoB(aiPlayer, nearUserPlayer);
            //SoundManager.GetInst().PlayAttackSound();

            //aiPlayer.anim.SetBool("Attack", true);

            return;
        }

        // 2-2. 못 찾으면 턴을 넘긴다
        pm.TurnOver();
    }
}
