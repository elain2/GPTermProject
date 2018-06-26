using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingTimeLineControl : MonoBehaviour {

    PlayableDirector director;
    public bool b_play = false;
	// Use this for initialization
	void Start () {
        director = GetComponent<PlayableDirector>();
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
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            director.Play();
            b_play = true;
        }
    }


}
