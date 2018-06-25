using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class OpeningSceneControl : MonoBehaviour {

    PlayableDirector director;

    public RawImage[] uiImage;
    bool CanvasColorCorStart = false;

	// Use this for initialization
	void Awake () {

        director = GetComponent<PlayableDirector>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (director.state != PlayState.Playing)
        {
            Camera.main.orthographic = true;
            Debug.Log("Its End");
            for(int i=0; i<uiImage.Length; i++)
            {
                uiImage[i].color = Color.Lerp(uiImage[i].color, new Color(uiImage[i].color.r, uiImage[i].color.g, uiImage[i].color.b, 0), Time.deltaTime * 2);
            }
        }
	}

}
