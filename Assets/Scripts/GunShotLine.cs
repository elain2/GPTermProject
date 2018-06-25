using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShotLine : MonoBehaviour {

    LineRenderer line;
    Ray gunRay;
    RaycastHit gunHit;
    float MaxLength = 100;
	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {


        Ray gunRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(gunRay, out gunHit))
        {
            Debug.Log("shot");
            line.SetPosition(0, transform.position);
            line.SetPosition(1, gunHit.point);
        }else
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, gunRay.direction * MaxLength);
        }
	}
}
