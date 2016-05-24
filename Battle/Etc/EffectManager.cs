using UnityEngine;
using System.Collections;

public class EffectManager {

    private static EffectManager inst = null;
    // todo : 코드상에서 적용해야 함
    public GameObject GO_AttackEffect; // unity에서 드래그로 설정한 공격 이펙트
    public GameObject go; // 남는 이펙트 삭제 위해
    public GameObject GO_Damage;

    public static EffectManager GetInst()
    {
        if (inst == null)
        {
            inst = new EffectManager();
            inst.Init();
        }

        return inst;
    }

    public void Init()
    {
        GO_AttackEffect = (GameObject)Resources.Load("Effects/Lightning Spark");

        GO_Damage = (GameObject)Resources.Load("Effects/Damage");
    }

    // todo: 이펙트를 화면에 보여주는데,차후 버전에서는 이펙트 종류나 타입 등을 설정할 수 있도록 바꿔야 한다
    public void ShowEffect(GameObject hex)
    {
        Vector3 pos = hex.transform.position;
        pos.y += 2f;
        go = (GameObject)GameObject.Instantiate(GO_AttackEffect, pos, hex.transform.rotation);
        //GameObject.Destroy(go);
    }

    public void ShowDamage(Hex hex, int damage)
    {
        GameManager.GetInst().damagedHex = hex;
        GameManager.GetInst().damage = damage;
        GameManager.GetInst().StartCoroutine("ShowDamage");
    }
}
