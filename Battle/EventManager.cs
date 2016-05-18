using UnityEngine;
using System.Collections;

public class EventManager {
    private static EventManager inst = null;

    public bool StageStartEvent = false;
    public bool StageStarted = false;
    public bool GameEnd = false;

    public static EventManager GetInst()
    {
        if (inst == null)
        {
            inst = new EventManager();
        }

        return inst;
    }

    public void ShowStartEvent()
    {
        GameManager.GetInst().StartCoroutine("ShowStageString");
        StageStartEvent = true;
    }
}
