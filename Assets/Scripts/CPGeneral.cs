using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CPGeneral : MonoBehaviour {

    public static CPGeneral instance = null;

	// Use this for initialization
	void Awake () {
		if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    private void Start()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
