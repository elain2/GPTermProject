using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class OpeningScene2Control : MonoBehaviour {

    public PlayableDirector director;
    public bool b_Play = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (b_Play)
        {
            if (director.state != PlayState.Playing)
            {
                Debug.Log("Its stop");
                SceneManager.LoadScene("Scene3",LoadSceneMode.Single);
                b_Play = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
        {
            b_Play = true;
            director.Play();
        }
    }
}
