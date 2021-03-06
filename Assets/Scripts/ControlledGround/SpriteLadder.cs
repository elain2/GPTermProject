﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLadder : ControlledMap {

    public Transform StartPoint, EndPoint;
    public float f_Speed = 4;

    SpriteRenderer sprRen;
    float f_Distance;
    Vector3 CenterPos;
    BoxCollider col;
    // Use this for initialization
    void Start () {
        f_Distance = Vector3.Distance(StartPoint.position, EndPoint.position);
        CenterPos = Vector3.Lerp(StartPoint.position, EndPoint.position, 0.5f);

        transform.position = StartPoint.position;
        sprRen = GetComponent<SpriteRenderer>();

        sprRen.size = new Vector2(sprRen.size.x, 0);
        col = GetComponent<BoxCollider>();
        col.size = new Vector3(col.size.x, 0, col.size.z);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (b_Start)
        {
            Debug.Log("F_Distance : " + f_Distance);
            MoveStart();
        }
        else
        {
            reverseStart();
        }
	}
    public override void MoveStart()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        transform.position = Vector3.Lerp(transform.position, CenterPos, f_Speed * Time.deltaTime);
        sprRen.size = Vector2.Lerp(sprRen.size, new Vector2(sprRen.size.x,f_Distance), f_Speed * Time.deltaTime);
        col.size = Vector3.Lerp(col.size, new Vector3(col.size.x, f_Distance, col.size.z), f_Speed * Time.deltaTime);

    }

    public void reverseStart()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = false;
        transform.position = Vector3.Lerp(transform.position, StartPoint.position, f_Speed * Time.deltaTime);
        sprRen.size = Vector2.Lerp(sprRen.size, new Vector2(sprRen.size.x, 0), f_Speed * Time.deltaTime);
        col.size = Vector3.Lerp(col.size, new Vector3(col.size.x, 0, col.size.z), f_Speed * Time.deltaTime);
    }

}
