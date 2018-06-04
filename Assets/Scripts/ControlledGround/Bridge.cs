using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : ControlledMap {

    public Transform StartPoint, EndPoint;
    public bool b_UseY = true;

	// Use this for initialization
	void Start () {
        MoveStart();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void MoveStart()
    {
        float Distance = Vector3.Distance(StartPoint.position, EndPoint.position);
        Vector3 CenterPos = Vector3.Lerp(StartPoint.position, EndPoint.position, 0.5f);
        transform.position = CenterPos;
        Debug.Log(GetComponent<MeshRenderer>().bounds.size.x);
        transform.localScale = new Vector3(transform.localScale.x / GetComponent<MeshRenderer>().bounds.size.x * Distance, transform.localScale.y, transform.localScale.z);
        float TempX = EndPoint.position.x - StartPoint.position.x;
        float TempZ = EndPoint.position.z - StartPoint.position.z;
        float TempDegree = Mathf.Atan2(TempX, TempZ) * Mathf.Rad2Deg;
        if (b_UseY)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -(90 - TempDegree), transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -(90 - TempDegree));
        }

    }
}
