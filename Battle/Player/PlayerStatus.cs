using UnityEngine;
using System.Collections;

public class PlayerStatus {
    public string Name = "test";
    public int CurHp = 100;
    public int MoveRange = 2; // 이동 범위
    public int AtkRange = 1; // 공격 범위
    public float MoveSpeed = 8f; // 이동 속도
    
    public PlayerStatus()
    {
        Name = "test";
        CurHp = 1;
        MoveRange = 2; // 이동 범위
        AtkRange = 1; // 공격 범위
        MoveSpeed = 6f; // 이동 속도
    }
}
