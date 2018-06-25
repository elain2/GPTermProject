using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineScriptControl : MonoBehaviour {

    public string[] TextSet;
    public int TextSetNum = 0;
    Text UIText;

	// Use this for initialization
	void Start () {
        TextSetNum = 0;
        UIText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(TextSet == null || TextSetNum >= TextSet.Length)
        {
            return;
        }else
        {
            UIText.text = TextSet[TextSetNum];
        }
	}
}
