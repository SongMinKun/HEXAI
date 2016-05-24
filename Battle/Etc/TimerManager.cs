using UnityEngine;
using System.Collections;

public class TimerManager : MonoBehaviour {
    private static TimerManager inst = null;

    private TextMesh timer;
    public float timerFortext;
    public int sec = 0, min = 1;

    public static TimerManager GetInst()
    {
        return inst;
    }

    void Awake()
    {
        inst = this;
    }

	// Use this for initialization
	void Start () {
        timer = gameObject.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
    void Update()
    {
        EventManager em = EventManager.GetInst();
        if (em.StageStarted == true)
        {
            timerFortext += Time.deltaTime;

            if (timerFortext > 1.0f)
            {
                sec--;
                if (sec < 0)
                {
                    min--;
                    sec = 59;
                    if (min < 0)
                    {
                        min = 1;
                        sec = 0;
                    }
                    
                }
                timer.text = string.Format("{0:D2}", min) + " : " + string.Format("{0:D2}", sec);
                timerFortext = 0;
            }
        }
    }

    public void resetTimer()
    {
        sec = 0;
        min = 1;
        timerFortext = 0;
        timer.text = string.Format("{0:D2}", min) + " : " + string.Format("{0:D2}", sec);
        Debug.Log("REset TIme");
    }
}
/*
public class Timer : MonoBehaviour {


    private GUIText _timer; // 타이머 GUI 텍스트 변수 선언
    private float _timerForText; // 시간 표현을 위한 실수 변수 선언
    private int _secText; //초 표현을 위한 정수 변수 선언
    private int _minText; //분 표현을 위한 정수 변수 선언


    // Use this for initialization
    void Start () {

        // GUIText 컴퍼넌트를 가져옴
        _timer = gameObject.GetComponent<GUIText>();
	
    }
	
    // Update is called once per frame
    void Update () {

        //타이머 계산
        _timerForText += Time.deltaTime;

        //타이버 발동
        if (_timerForText > 1.0f)
        {
            _secText += 1; // 초 증가
            if (_secText > 60) // 분 계산 조건
            {
                _minText += 1; // 분 증가
                _secText = 0; // 초 리셋
            }
            // 1자리 숫자경우 앞에 0을 붙이기 위한 포맷 변환구문
            _timer.text = string.Format("{0:D2}", _minText) + ":" + string.Format("{0:D2}", _secText);
            _timerForText = 0; // 타이머 리셋
        }

	
    }
}

*/