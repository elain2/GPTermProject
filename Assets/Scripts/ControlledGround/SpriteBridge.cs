using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBridge : ControlledMap {

    public Transform StartPoint, EndPoint;

    public float f_Speed = 4;

    SpriteRenderer sprRen;
    Transform BridgeCube;

    float f_Distance;
    Vector3 CenterPos;
    Vector3 ScaleGoal;

    // Use this for initialization
    void Start()
    {
        f_Distance = Vector3.Distance(StartPoint.position, EndPoint.position);
        CenterPos = Vector3.Lerp(StartPoint.position, EndPoint.position, 0.5f);

        transform.position = StartPoint.position;
        sprRen = GetComponent<SpriteRenderer>();

        sprRen.size = new Vector2(sprRen.size.x, 0);

        //if (transform.Find("BridgeCube") != null)
        //{
        //    BridgeCube = transform.Find("BridgeCube");
        //}


        GetComponent<Renderer>().enabled = false;


    //    BridgeCube.GetComponent<Renderer>().enabled = false;

    //    BridgeCube.GetComponent<Collider>().enabled = false;

    //    ScaleGoal = new Vector3(BridgeCube.localScale.x, f_Distance,
    //BridgeCube.localScale.z);

    }
	
	// Update is called once per frame
	void Update () {
        if (b_Start)
        {
            MoveStart();
        }else
        {

        }
	}

    public override void MoveStart()
    {
        GetComponent<Renderer>().enabled = true;

        //BridgeCube.GetComponent<Renderer>().enabled = true;

        //BridgeCube.GetComponent<Collider>().enabled = true;

        sprRen.size = Vector2.Lerp(sprRen.size, new Vector2(sprRen.size.x, f_Distance), f_Speed * Time.deltaTime);
        //BridgeCube.position = Vector3.Lerp(BridgeCube.position, CenterPos, f_Speed * Time.deltaTime);
        //BridgeCube.localScale = Vector3.Lerp(BridgeCube.localScale, ScaleGoal, f_Speed * Time.deltaTime);
    }


}
