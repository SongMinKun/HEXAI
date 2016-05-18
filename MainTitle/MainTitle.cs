using UnityEngine;
using System.Collections;

public class MainTitle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {
        float btnW = 150f;
        float btnH = 50f;

        Rect rect = new Rect(Screen.width / 2 - btnW / 2, Screen.height / 2, btnW, btnH);

        if (GUI.Button(rect, "New Game"))
        {
            Application.LoadLevel(1);
        }
    }
}
