using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingTimeLineControl : MonoBehaviour {

    PlayableDirector director;
    public bool b_play = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (b_play)
        {
            if(director.state != PlayState.Playing)
            {
                Application.Quit();
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
        {

            other.GetComponent<PlayerPhysics>().b_CanControl = false;
            director.Play();
            b_play = true;
        }
    }


}
