using UnityEngine;
using System.Collections;

public enum UNIT
{
    KING,
    WARRIOR,
    TANKER,
    MAGICIAN,
    ARCHER,
    HEALER
}

public enum SKIL
{
    KING_PASSIVE,       // 아군 전멸시 Power * 3
    WARRIOR_ACTIVE,     // 1.5 배의 공격력으로 공격
    TANKER_PASSIVE,     // 체력 1.5배
    MAGICIAN_ACTIVE,    // 2,5 배의 공격력으로 단일 공격
    ARCHER_ACTIVE,      // ATKRANGE 5로 증가
    HEALER_ACTIVE       // 힐
}

public class PlayerStatus {
    public string Name;

    public int CurHp;
    public int MaxHp;
    public int MoveRange;
    public int AtkRange;
    public int Power;
    public SKIL Skil;

    public float MoveSpeed = 6f;
    
    public PlayerStatus()
    {
        Name = "test";
        CurHp = 1;
        MoveRange = 2; // 이동 범위
        AtkRange = 1; // 공격 범위
        Power = 10;
    }

    public PlayerStatus(UNIT unit)
    {
        if (unit == UNIT.KING)
        {
            Name = "KING";
            MaxHp = 1000;
            CurHp = 1000;
            Power = 100;
            MoveRange = 1;
            AtkRange = 1;
            Skil = SKIL.KING_PASSIVE;
        }
        else if (unit == UNIT.WARRIOR)
        {
            Name = "WARRIOR";
            MaxHp = 1250;
            CurHp = 1250;
            Power = 300;
            MoveRange = 2;
            AtkRange = 1;
            Skil = SKIL.WARRIOR_ACTIVE;
        }
        else if (unit == UNIT.TANKER)
        {
            Name = "TANKER";
            MaxHp = 1500;
            CurHp = 1500;
            Power = 200;
            MoveRange = 1;
            AtkRange = 1;
            Skil = SKIL.TANKER_PASSIVE;
        }
        else if (unit == UNIT.MAGICIAN)
        {
            Name = "MAGICIAN";
            MaxHp = 900;
            CurHp = 900;
            Power = 188;
            MoveRange = 2;
            AtkRange = 2;
            Skil = SKIL.MAGICIAN_ACTIVE;
        }
        else if (unit == UNIT.ARCHER)
        {
            Name = "ARCHER";
            MaxHp = 800;
            CurHp = 800;
            Power = 275;
            MoveRange = 1;
            AtkRange = 3;
            Skil = SKIL.ARCHER_ACTIVE;
        }
        else if (unit == UNIT.HEALER)
        {
            Name = "HEALER";
            MaxHp = 1000;
            CurHp = 1000;
            Power = 87;
            MoveRange = 2;
            AtkRange = 1;
            Skil = SKIL.HEALER_ACTIVE;
        }
        else
        {
            Name = "ERROR";
            MaxHp = 1;
            CurHp = 1;
            Power = 1;
            MoveRange = 1;
            AtkRange = 1;
        }
    }
}
