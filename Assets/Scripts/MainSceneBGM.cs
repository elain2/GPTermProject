using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneBGM : MonoBehaviour {

    public AudioClip[] BGM;

    AudioSource BGM_Audio;

	void Start ()
    {
        BGM_Audio = GetComponent<AudioSource>();
        BGM_Audio.clip = BGM[0];
        BGM_Audio.Play();
    }
	
	void Update ()
    {
        if (BGM_Audio.isPlaying == false)
        {
            StartCoroutine("PlayBGM");
        }
	}

    IEnumerator PlayBGM()
    {
        BGM_Audio.clip = BGM[Random.Range(0, 2)];
        BGM_Audio.Play();

        yield return null;
    }
}
