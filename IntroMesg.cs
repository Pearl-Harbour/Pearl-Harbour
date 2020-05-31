using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMesg : MonoBehaviour {
    private FPSPlayer fps;
	// Use this for initialization
	void Start () {
       // Time.timeScale = 1f;
        Invoke("UpdateHeight", 0.1f);
        fps = FindObjectOfType<FPSPlayer>();
	}
	void UpdateHeight()
    {
      //  Debug.Log("wow");
        if (GetComponent<RectTransform>().position.y<810)
        Invoke("UpdateHeight", 0.1f);
        else
        {
            PlayerPrefs.SetInt("Introduction", 1);
            fps.levelLoadFadeObj.GetComponent<LevelLoadFade>().FadeAndLoadLevel(Color.black, 1.2f, false);
        }
        GetComponent<RectTransform>().position = new Vector2(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y + 2);
    }
}
