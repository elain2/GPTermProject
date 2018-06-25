using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip JumpSound;
    public AudioClip DeadSound;
    public AudioClip WalkSound;

    AudioSource myAudio;

    public static PlayerSound instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

	// Use this for initialization
	void Start ()
    {
        myAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Jump()
    {
        myAudio.PlayOneShot(JumpSound);
    }

    public void Dead()
    {
        myAudio.PlayOneShot(DeadSound);
    }

    public void Walk()
    {
        myAudio.PlayOneShot(WalkSound);
    }
}
