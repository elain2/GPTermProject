using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody rigid;
    BoxCollider col;
    Transform SpriteObj;
    Animator anim;
    public float f_Speed = 3;
    public float f_JumpSpeed = 10;
    public bool b_canMoveY = false;
    public bool b_Falling = false;

    public bool b_Jump = false;

    Vector3 RayPoint1;
    Vector3 RayPoint2;
    Vector3 RayPoint3;
 
    Ray Ray1;
    Ray Ray2;
    Ray Ray3;
    RaycastHit hit, hit2, hit3;
    Vector3 dir;

    CameraScript camScript;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        camScript = Camera.main.GetComponent<CameraScript>();
        anim = GetComponentInChildren<Animator>();
        SpriteObj = transform.Find("Sprite");

	}
	
	// Update is called once per frame
	void Update () {

        transform.eulerAngles = new Vector3(0,  Camera.main.transform.eulerAngles.y, 0);
        MoveFunc();
        FallingCheck();
        AnimationControl();
        
	}

    void Billboard()
    {
        
    }

    void MoveFunc()
    {
        int tempX = 0;
        int tempY = 0;
        int tempZ = 0;
        Vector3 StartPoint = col.bounds.center;

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            if (camScript.f_CurDegree == 0 || camScript.f_CurDegree == 360 || camScript.f_CurDegree == -360)
            {
                dir = Vector3.left;
            }
            else if (camScript.f_CurDegree == 180 || camScript.f_CurDegree == -180)
            {
                dir = Vector3.right;

            }
            else if (camScript.f_CurDegree == 90 || camScript.f_CurDegree == -270)
            {
                dir = Vector3.back;
            }
            else
            {
                dir = Vector3.forward;
            }

            if (camScript.f_CurDegree == 0 || camScript.f_CurDegree == 360 || camScript.f_CurDegree == -360)
            {
                RayPoint1 = StartPoint + new Vector3(-col.bounds.size.x / 2 + 0.05f, col.bounds.size.y / 2);
                RayPoint2 = StartPoint + new Vector3(-col.bounds.size.x / 2 + 0.05f, 0);
                RayPoint3 = StartPoint + new Vector3(-col.bounds.size.x / 2 + 0.05f, -col.bounds.size.y / 2);

            }else if (Mathf.Abs(camScript.f_CurDegree) == 180)
            {
                RayPoint1 = StartPoint + new Vector3(col.bounds.size.x / 2 - 0.05f, col.bounds.size.y / 2);
                RayPoint2 = StartPoint + new Vector3(col.bounds.size.x / 2 - 0.05f, 0);
                RayPoint3 = StartPoint + new Vector3(col.bounds.size.x / 2 - 0.05f, -col.bounds.size.y / 2);
            }
            else if(camScript.f_CurDegree == 90 || camScript.f_CurDegree == -270)
            {

                RayPoint1 = StartPoint + new Vector3(0, col.bounds.size.y / 2, -col.bounds.size.z / 2 + 0.05f);
                RayPoint2 = StartPoint + new Vector3(0, 0, -col.bounds.size.z / 2 + 0.05f);
                RayPoint3 = StartPoint + new Vector3(0, -col.bounds.size.y / 2, -col.bounds.size.z / 2 + 0.05f);
            }
            else
            {
                RayPoint1 = StartPoint + new Vector3(0, col.bounds.size.y / 2, col.bounds.size.z / 2 + 0.05f);
                RayPoint2 = StartPoint + new Vector3(0, 0, col.bounds.size.z / 2 + 0.05f);
                RayPoint3 = StartPoint + new Vector3(0, -col.bounds.size.y / 2, col.bounds.size.z / 2 + 0.05f);

            }
            Ray1 = new Ray(RayPoint1, dir);
            Ray2 = new Ray(RayPoint2, dir);
            Ray3 = new Ray(RayPoint3, dir);
            Debug.DrawRay(RayPoint1, dir);
            Debug.DrawRay(RayPoint2, dir);
            Debug.DrawRay(RayPoint3, dir);
            if (Physics.Raycast(Ray1,out hit, 0.1f) || Physics.Raycast(Ray2, out hit, 0.1f) || Physics.Raycast(Ray3, out hit, 0.1f))
            {
                if(hit.collider.transform.gameObject.layer != LayerMask.NameToLayer("Ground"))
                {

                    tempX -= 1;
                }
              

            }
            else
            {
                tempX -= 1;
            }

        }
        if (Input.GetKey(KeyCode.RightArrow) )
        {

            if(camScript.f_CurDegree == 0 || camScript.f_CurDegree == 360 || camScript.f_CurDegree == -360)
            {
                dir = Vector3.right;
            }else if(camScript.f_CurDegree == 180 || camScript.f_CurDegree == -180)
            {
                dir = Vector3.left;
            }else if(camScript.f_CurDegree == 90 || camScript.f_CurDegree == -270)
            {
                dir = Vector3.forward;
            }else
            {
                dir = Vector3.back;
            }

            if (camScript.f_CurDegree == 0 || camScript.f_CurDegree == 360 || camScript.f_CurDegree == -360)
            {
                RayPoint1 = StartPoint + new Vector3(col.bounds.size.x / 2 - 0.05f, col.bounds.size.y / 2);
                RayPoint2 = StartPoint + new Vector3(col.bounds.size.x / 2 - 0.05f, 0);
                RayPoint3 = StartPoint + new Vector3(col.bounds.size.x / 2 - 0.05f, -col.bounds.size.y / 2);
            }
            else if (Mathf.Abs(camScript.f_CurDegree) == 180)
            {
                RayPoint1 = StartPoint + new Vector3(-col.bounds.size.x / 2 + 0.05f, col.bounds.size.y / 2);
                RayPoint2 = StartPoint + new Vector3(-col.bounds.size.x / 2 + 0.05f, 0);
                RayPoint3 = StartPoint + new Vector3(-col.bounds.size.x / 2 + 0.05f, -col.bounds.size.y / 2);
            }
            else if (camScript.f_CurDegree == 90 || camScript.f_CurDegree == -270)
            {
                RayPoint1 = StartPoint + new Vector3(0, col.bounds.size.y / 2, col.bounds.size.z / 2 - 0.05f);
                RayPoint2 = StartPoint + new Vector3(0, 0, col.bounds.size.z / 2 - 0.05f);
                RayPoint3 = StartPoint + new Vector3(0, -col.bounds.size.y / 2, col.bounds.size.z / 2 - 0.05f);
            }
            else
            {

                RayPoint1 = StartPoint + new Vector3(0, col.bounds.size.y / 2, -col.bounds.size.z / 2 - 0.05f);
                RayPoint2 = StartPoint + new Vector3(0, 0, -col.bounds.size.z / 2 - 0.05f);
                RayPoint3 = StartPoint + new Vector3(0, -col.bounds.size.y / 2, -col.bounds.size.z / 2 - 0.05f);
            }
            Ray1 = new Ray(RayPoint1, dir);
            Ray2 = new Ray(RayPoint2, dir);
            Ray3 = new Ray(RayPoint3, dir);

            if (Physics.Raycast(Ray1, out hit, 0.1f) || Physics.Raycast(Ray2, out hit, 0.1f) || Physics.Raycast(Ray3, out hit, 0.1f))
            {
                if (hit.collider.transform.gameObject.layer != LayerMask.NameToLayer("Ground"))
                {
                    tempX += 1;
                }
            }
            else
            {
                tempX += 1;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            tempY -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            tempY += 1;
        }
        if(Input.GetKeyDown(KeyCode.Space) && !b_Jump && !Input.GetKey(KeyCode.DownArrow))
        {
            b_Jump = true;
            rigid.velocity = new Vector3(0, f_JumpSpeed); 
        }
        
        if(camScript.f_CurDegree == 0 || camScript.f_CurDegree == 360 || camScript.f_CurDegree == -360)
        {
            tempZ = 0;
        }else if(Mathf.Abs(camScript.f_CurDegree) == 180)
        {
            tempX = -tempX;
            tempZ = 0;
        }
        else if(camScript.f_CurDegree == 90 || camScript.f_CurDegree == -270)
        {
            tempZ = tempX;
            tempX = 0;
        }
        else if(camScript.f_CurDegree == -90 || camScript.f_CurDegree == 270)
        {
            tempZ = -tempX;
            tempX = 0;
        }

        //사다리타기 등등
        if (!b_canMoveY)
        {
            
            rigid.useGravity = true;
            rigid.velocity = new Vector3(tempX * f_Speed, rigid.velocity.y, tempZ * f_Speed);
        }else
        {
            rigid.useGravity = false;
            rigid.velocity = new Vector3(tempX * f_Speed, tempY * f_Speed, tempZ * f_Speed);
        }
    }

    void FallingCheck()
    {
        if (!b_canMoveY)
        {
            Vector3 StartPoint = new Vector3(col.bounds.center.x, col.bounds.center.y - col.bounds.size.y / 2 + 0.02f, col.bounds.center.z);

            Vector3 rayPoint1 = StartPoint + new Vector3(col.bounds.size.x / 2, 0, 0);
            Vector3 rayPoint2 = StartPoint - new Vector3(col.bounds.size.x / 2, 0, 0);
            Vector3 rayPoint3 = StartPoint + new Vector3(0, 0, col.bounds.size.z / 2);
            Vector3 rayPoint4 = StartPoint - new Vector3(0, 0, col.bounds.size.z / 2);

            RaycastHit hit;
            Ray ray1 = new Ray(rayPoint1, Vector3.down);
            Ray ray2 = new Ray(rayPoint2, Vector3.down);
            Ray ray3 = new Ray(rayPoint3, Vector3.down);
            Ray ray4 = new Ray(rayPoint4, Vector3.down);


            if (Physics.Raycast(ray1, out hit, 0.1f) || Physics.Raycast(ray2, out hit, 0.1f) || Physics.Raycast(ray3, out hit, 0.1f) || Physics.Raycast(ray4, out hit, 0.1f))
            {


                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    b_Falling = false;
                    b_Jump = false;

                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    b_Falling = false;
                    b_Jump = false;

                }

                else
                {
                    b_Falling = true;
                    b_Jump = true;
                }
            }
            else
            {
                b_Falling = true;
                b_Jump = true;
            }

        }else
        {
            b_Falling = false;
            b_Jump = false;
        }
    }

    void WalkonPlatform(RaycastHit _hit)
    {
        Vector3 StartPos = transform.position;
        transform.position = new Vector3(transform.position.x, col.bounds.size.y/2 + _hit.collider.bounds.center.y + _hit.collider.bounds.size.y, transform.position.z);
        if(Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, _hit.collider.bounds.center.y, transform.position.z);
        }
        
    }

    void AnimationControl()
    {

        if (rigid.velocity.x > 0 || rigid.velocity.z > 0)
        {
            if (Mathf.Abs(camScript.f_CurDegree) == 180 || camScript.f_CurDegree == -90 || camScript.f_CurDegree == 270)
            {
                SpriteObj.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                SpriteObj.localScale = new Vector3(-1, 1, 1);
            }
        }else if(rigid.velocity.x < 0 || rigid.velocity.z < 0)
        {
            if (Mathf.Abs(camScript.f_CurDegree) == 180 || camScript.f_CurDegree == -90 || camScript.f_CurDegree == 270)
            {
                SpriteObj.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                SpriteObj.localScale = new Vector3(1, 1, 1);
            }
        }
        
        if (b_Falling)
        {
            if(rigid.velocity.y > 0)
            {
                anim.Play("Jump_Up");
            }else
            {
                anim.Play("Jump_Down");
            }
        }else
        {
            if(rigid.velocity.x != 0 || rigid.velocity.z != 0)
            {
                anim.Play("Run");
            }
            else
            {
                Debug.Log("idle");
                anim.Play("Idle");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.gameObject.layer == LayerMask.NameToLayer("Grab"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                b_canMoveY = true;

            }
            else if(Input.GetKeyDown(KeyCode.Space)){
                b_canMoveY = false;
            }
        }

    }
    

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.gameObject.layer == LayerMask.NameToLayer("Grab"))
        {
            b_canMoveY = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            //if (collision.transform.position.y + collision.collider.bounds.size.y / 2 <= col.bounds.center.y - col.bounds.size.y / 2 + 0.1f)
            //{
            //    transform.parent = collision.transform;
            //}
        }
    }


}
