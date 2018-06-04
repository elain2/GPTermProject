using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public bool b_CameraFollow = true;
    public Transform player;
    private float f_Radius;
    public int f_CurDegree;
    private float f_tempRadius;
    Quaternion tempquat = new Quaternion();

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
        f_Radius = Mathf.Abs(player.position.z - Camera.main.transform.position.z);
        f_CurDegree = 0;
        f_tempRadius = 0;
    }

    // Update is called once per frame
    void Update () {

        if (b_CameraFollow)
        {


            if (Input.GetKeyDown(KeyCode.A))
            {
                if (f_CurDegree <= -360)
                {
                    f_CurDegree = 0;
                }
                f_CurDegree -= 90;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (f_CurDegree >= 360)
                {
                    f_CurDegree = 0;
                }
                f_CurDegree += 90;
            }


            RotateCamera(f_CurDegree);
        }
	}
    void RotateCamera(int degree)
    {
        tempquat.eulerAngles = new Vector3(0, -degree, 0);


        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, tempquat, 10 * Time.deltaTime);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.position.x + f_Radius * Mathf.Sin(degree * Mathf.Deg2Rad), player.position.y, player.position.z - f_Radius * Mathf.Cos(degree * Mathf.Deg2Rad)), 10 * Time.deltaTime);
        //if (degree != f_tempRadius)
        //{
        //    Debug.Log("Not Right " + "Camera : " + Camera.main.transform.eulerAngles.y + " Degree : " + (-degree));
        //    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.position.x + f_Radius * Mathf.Sin(degree * Mathf.Deg2Rad), player.position.y, player.position.z - f_Radius * Mathf.Cos(degree * Mathf.Deg2Rad)), 10 * Time.deltaTime);
        //    if((Mathf.Round(Camera.main.transform.eulerAngles.y) == -degree) || (Mathf.Round(Camera.main.transform.eulerAngles.y) == (360 - degree)))
        //    {
        //        f_tempRadius = degree;
        //    }
        //} else
        //{
        //    Debug.Log("Right");
        //    Camera.main.transform.position = new Vector3(player.position.x + f_Radius * Mathf.Sin(degree * Mathf.Deg2Rad), player.position.y, player.position.z - f_Radius * Mathf.Cos(degree * Mathf.Deg2Rad));
        //}


    }
}
