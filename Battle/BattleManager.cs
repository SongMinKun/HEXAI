using UnityEngine;
using System.Collections;

public class BattleManager {
    private static BattleManager inst = null;

    private float normalAttackTime = 0f;
    private PlayerBase attacker = null;
    private PlayerBase defender = null;

    public static BattleManager GetInst(){
        if (inst == null)
            inst = new BattleManager();
        return inst;
    }

    /*
     *  어떻게 쓰는 물건인고
     */

    // 여기도 바꾸고

    // 내가 바꿈 SMK
    // cjh1205 : 나도 바꿈
    // cjh1205 : 왜 안되지
    // 자러갑시다 님들 - psw221

	// Update is called once per frame
	public void CheckBattle () {    //todo : 이부분을 호출하는 부분이 필요함
        if (normalAttackTime != 0)
        {
            normalAttackTime += Time.smoothDeltaTime;
            if (normalAttackTime >= 0.5f)
            {
                normalAttackTime = 0f;

                Debug.Log("attack !! " + attacker.status.Name + " to " + defender.status.Name);
                defender.GetDamage(10);
                EffectManager.GetInst().ShowEffect(defender.gameObject);
                EffectManager.GetInst().ShowDamage(defender.CurHex, 10);

                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);

                PlayerManager.GetInst().SetTurnOverTime(1.5f);
   
            }
        }
	}

    public void AttackAtoB(PlayerBase a, PlayerBase b)
    {
        a.transform.rotation = Quaternion.LookRotation((b.CurHex.transform.position - a.transform.position).normalized);
        a.anim.SetBool("Attack", true);

        a.act = ACT.ATTACKING;
        normalAttackTime = Time.smoothDeltaTime;

        attacker = a;
        defender = b;
    }
}
