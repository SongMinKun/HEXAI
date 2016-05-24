using UnityEngine;
using System.Collections;

public class BattleManager {

    private static BattleManager inst = null;
    private PlayerBase attacker = null;
    private PlayerBase defender = null;

    private float normalAttackTime = 0;

    public static BattleManager GetInst() {
        if (inst == null)
        {
            inst = new BattleManager();
        }

        return inst;
    }
	
	// Update is called once per frame
    // todo : 호출하는 부분 필요함
	public void CheckBattle() {
        if(normalAttackTime != 0) {
            normalAttackTime += Time.smoothDeltaTime;

            //if (normalAttackTime >= 0.16f)
            if (normalAttackTime >= 0.16f)
            {
                normalAttackTime = 0f;
                
                // 데미지를 받는 부분 처리
                int damage = attacker.status.Power;
                
                // 데미지를 status의 power로 받도록 설정.
                defender.GetDamage(damage);
                defender.anim.SetTrigger("Hited");

                EffectManager.GetInst().ShowEffect(defender.gameObject);
                EffectManager.GetInst().ShowDamage(defender.CurHex, damage);

                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);


                PlayerManager.GetInst().SetTurnOverTime(1.5f);
            }
        }
	
	}

    public void AttackAtoB(PlayerBase a, PlayerBase b)
    {
        // 노말 어택이 이펙트 되는 시점은 0.18초
        a.transform.rotation = Quaternion.LookRotation((b.CurHex.transform.position - a.transform.position).normalized);
        a.anim.SetTrigger("Attack");
        a.act = ACT.ATTACKING;

        Debug.Log("ACT : attack " + a.ToString() + " to " + b.ToString());

        normalAttackTime = Time.smoothDeltaTime;
        attacker = a;
        defender = b;
    }
}
