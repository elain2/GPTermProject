using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paricleCubeBehaviour : MonoBehaviour {
    public GameObject camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Vector3 cubePosition;
    void LateUpdate()
    {
        cubePosition.x = camera.transform.position.x;
        cubePosition.y = camera.transform.position.y;
        cubePosition.z = camera.transform.position.z;
        transform.position = cubePosition;
    }
}
