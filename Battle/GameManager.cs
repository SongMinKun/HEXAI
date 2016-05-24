using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager inst = null;
    MapManager mm;
    PlayerManager pm;
    SoundManager sm;
    BattleManager bm;
    GUIManager gm;
    EventManager em;

    GameObject StageStartString;
    GameObject StageClearString;
    GameObject GameOverString;

    public static GameManager GetInst()
    {
        return inst;
    }

    void Awake()
    {
        inst = this;
        mm = MapManager.GetInst();
        pm = PlayerManager.GetInst();
        sm = SoundManager.GetInst();
        bm = BattleManager.GetInst();
        gm = GUIManager.GetInst();
        em = EventManager.GetInst();

        StageStartString = (GameObject)GameObject.FindGameObjectWithTag("StageStart");
        StageClearString = (GameObject)GameObject.FindGameObjectWithTag("StageClear");
        GameOverString = (GameObject)GameObject.FindGameObjectWithTag("GameOver");

        StageClearString.SetActive(false);
        GameOverString.SetActive(false);

        Screen.SetResolution(1024, 720, true);
    }

	// Use this for initialization
	void Start () {
        mm.CreateMap();
        pm.GenPlayerTest();
        sm.PlayMusic(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        if(em.GameEnd == true)
        {
            return;
        }

        if (em.StageStarted == true)
        {
            CheckMouseButtonDown();
            bm.CheckBattle();
            pm.CheckTurnOver();
        }

        else if(em.StageStartEvent == false)
        {
            em.ShowStartEvent();
        }
	}

    void OnGUI()
    {
        if (em.StageStarted)
        {
            gm.DrawGUI();
        }
    }

    // 마우스 우클릭시 캔슬
    void CheckMouseButtonDown()
    {
        // 마우스 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse 1(Right) down!!");
            pm.MouseInputProc(1);
        }
    }

    public Hex damagedHex;
    public int damage;

    IEnumerator ShowDamage()
    {
        GameObject GO_Damage = (GameObject)Resources.Load("Effects/Damage");
        GameObject obj = (GameObject)GameObject.Instantiate(GO_Damage, damagedHex.transform.position, GameManager.GetInst().gameObject.transform.rotation);
        
        TextMesh tm = obj.GetComponent<TextMesh>();
        tm.text = damage.ToString();

        yield return new WaitForSeconds(0.5f);

        for (float i = 1; i >= 0; i -= 0.05f)
        {
            tm.color = new Vector4(255, 0, 0, i);

            yield return new WaitForFixedUpdate();
        }

        Destroy(obj);
    }

    IEnumerator ShowStageString()
    {
        yield return new WaitForSeconds(3f);

        Destroy(StageStartString);
        em.StageStarted = true;
    }

    public void ShowStageClear()
    {
        StageClearString.SetActive(true);
    }

    public void ShowGameOver()
    {
        GameOverString.SetActive(true);
    }
}
