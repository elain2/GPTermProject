using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public bool b_CameraFollow = true;
    public bool b_CameraMove = true;
    public Transform player;
    public float f_Radius = 6;
    public int f_CurDegree;
    public float CameraHeight = 5;
    public float lerpSpeed = 6;
    private float f_tempRadius;
    Quaternion tempquat = new Quaternion();


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
        //f_Radius = Mathf.Abs(player.position.z - Camera.main.transform.position.z);

        f_tempRadius = 0;
    }

    // Update is called once per frame
    void Update () {

        if (b_CameraFollow)
        {

            if (b_CameraMove)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (f_CurDegree <= -360)
                    {
                        f_CurDegree = 0;
                    }
                    f_CurDegree -= 90;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (f_CurDegree >= 360)
                    {
                        f_CurDegree = 0;
                    }
                    f_CurDegree += 90;
                }
            }


            RotateCamera(f_CurDegree);
        }
	}
    void RotateCamera(int degree)
    {

        
        Vector3 CamtoPlayer = Vector3.Normalize(Camera.main.transform.position - player.transform.position);
        float angle = Vector3.Angle(Vector3.up, CamtoPlayer);

        tempquat.eulerAngles = new Vector3(90-angle, -degree, 0);


        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, tempquat, lerpSpeed * Time.deltaTime);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.position.x + f_Radius * Mathf.Sin(degree * Mathf.Deg2Rad), player.position.y + CameraHeight, player.position.z - f_Radius * Mathf.Cos(degree * Mathf.Deg2Rad)), lerpSpeed * Time.deltaTime);
        
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
