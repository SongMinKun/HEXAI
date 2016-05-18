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

// Player을 관리하는
public class PlayerBase : MonoBehaviour {

    public Animator anim;
    public PlayerStatus status;

    public Hex CurHex; // 어느 위치에 있는지 Hex 저장
    public ACT act;
    public List<Hex> MoveHexes;

    public float removeTime = 0;

    public bool enemyInfo = false;
    public PlayerBase enemy;
    
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

    void OnGUI()
    {
        if (enemyInfo)
            DrawEnemyInfo();
    }


    /*
    public virtual void DrawStatus()
    {

    }

    public virtual void DrawCommand()
    {

    }
     */
    void DrawEnemyInfo()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 4, Screen.height / 4 * 3, 150f, Screen.height / 4), "Player Info", GUI.skin.window);

        GUILayout.Label("Name : " + enemy.status.Name);
        GUILayout.Label("HP : " + enemy.status.CurHp);
        GUILayout.Label("MoveRange : " + enemy.status.MoveRange);
        GUILayout.Label("AtkRange : " + enemy.status.AtkRange);

        GUILayout.EndArea();
    }


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
