using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : ControlledMap {

    public Transform StartPoint, EndPoint;

    //종종 X축이 90 도로 뒤집혀 있는 놈이 있음(3D 모델) 이것을 위해 존재함. 그냥 일반 오브젝트는 true
    public bool b_UseY = true;
    public float f_Speed = 4;
    float f_Distance;
    Vector3 CenterPos;
    Vector3 ScaleGoal;
    float f_BoundX, f_BoundY;

	// Use this for initialization
	void Start () {
        f_Distance = Vector3.Distance(StartPoint.position, EndPoint.position);
        CenterPos = Vector3.Lerp(StartPoint.position, EndPoint.position, 0.5f);
        transform.position = StartPoint.position;

        if (StartPoint.position.y == EndPoint.position.y)
        {
            ScaleGoal = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / GetComponent<MeshRenderer>().bounds.size.z * f_Distance);
            transform.localScale -= new Vector3(0, 0, transform.localScale.z);
            float TempX = EndPoint.position.x - StartPoint.position.x;
            float TempZ = EndPoint.position.z - StartPoint.position.z;
            float TempDegree = Mathf.Atan2(TempX, TempZ) * Mathf.Rad2Deg;
            if (b_UseY)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, TempDegree, transform.eulerAngles.z);
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, TempDegree);
            }
        }else
        {
            ScaleGoal = new Vector3(transform.localScale.x, transform.localScale.y / GetComponent<MeshRenderer>().bounds.size.y * f_Distance, transform.localScale.z);
            transform.localScale -= new Vector3(0, - transform.localScale.y, 0);

        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
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
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.position = Vector3.Lerp(transform.position, CenterPos, f_Speed * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, ScaleGoal, f_Speed * Time.deltaTime);
        if(transform.position == CenterPos)
        {
            Debug.Log("Bridge End");
            this.enabled = false;
        }
    }

    
}
