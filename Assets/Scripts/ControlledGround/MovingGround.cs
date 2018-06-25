using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : ControlledMap {

    public Transform EndPoint;
    Vector3 StartPoint;
    public float f_Speed = 4;
    bool b_GotoEnd = true;



	// Use this for initialization
	void Start () {
        StartPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (b_Start)
        {
            MoveStart();
        }
	}
    public override void MoveStart()
    {
        if (b_GotoEnd)
        {
            if(transform.position != EndPoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, EndPoint.position, Time.deltaTime * f_Speed);
            }else
            {
                b_GotoEnd = false;
            }
        }else
        {
            if(transform.position != StartPoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, StartPoint, Time.deltaTime * f_Speed);
            }else
            {
                b_GotoEnd = true;
            }
        }
    }
}
