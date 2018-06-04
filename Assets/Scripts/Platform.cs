using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    Transform player;
    BoxCollider playercol;
    BoxCollider col;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playercol = player.GetComponent<BoxCollider>();
        col = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if ((col.bounds.center.y + col.bounds.size.y * 0.5f) <= (playercol.bounds.center.y - playercol.bounds.size.y * 0.5f) + 0.25f)
        {
            col.isTrigger = false;
        }else
        {

            col.isTrigger = true;
        }
	}
}
