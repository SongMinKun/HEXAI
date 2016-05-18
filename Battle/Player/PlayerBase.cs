using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 완료
public enum ACT
{
    IDLE,
    MOVEHIGHLIGHT,
    MOVING,
    ATTACKHIGHLIGHT,
    ATTACKING,
    DIED
}
/// <summary>
/*
/// </summary>
public enum JOB
{
    KING,
    KNIGHT,
    WARRIOR,
    ARCHER,
    HEALER,
    MAGICIAN
}

// todo : 스킬은 나중에 따로 클래스로 만들어서 유지 보수 편하게
public class Unit
{
    public JOB job;     // 직업
    public int hp;      // 체력
    public int power;   // 공격력
    public int skill;   // 스킬
    public int coolTime; // 스킬 쿨타임
    public int moveRange; // 이동 범위
    public int attackRange; // 공격 범위
    public int skillRange; // 스킬 범위
}
*/
// Player을 관리하는
public class PlayerBase : MonoBehaviour {

    public Animator anim;
    public PlayerStatus status;

    public Hex CurHex; // 어느 위치에 있는지 Hex 저장
    public ACT act;
    public List<Hex> MoveHexes;

    public float removeTime = 0;
    
    // todo : 같은 편의 말이면 지나갈 수 있지만 상대방의 밀이면 지나갈 수 없게

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
	
	}
	
    // 완료
	// Update is called once per frame
    // 플레이어의 이동시 모습을 스무스하게 구현
	void Update () {

	}

    /*
    public virtual void DrawStatus()
    {

    }

    public virtual void DrawCommand()
    {

    }
     */

    public void GetDamage(int damage)
    {
        status.CurHp -= damage;

        if (status.CurHp <= 0)
        {
            Debug.Log("Died!!");
            anim.SetTrigger("Die");
            act = ACT.DIED;

            removeTime += Time.deltaTime;
            //PlayerManager.GetInst().RemovePlayer(this);
        }

        else
        {
            Debug.Log("Hited");
            anim.SetTrigger("Hited");
        }
    }
}
