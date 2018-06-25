using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPhysics : MonoBehaviour {

    Rigidbody rigid;
    public float speed = 4;
    public float f_Health = 3;
    public float f_JumpHeight = 10;
    public float f_DeathHeight = 25;
    //애초에 조작을 할 수 있는 상태인가?
    public bool b_CanControl = true;

    int tempX = 0;
    int tempZ = 0;
    bool b_isBack = false;
    float cur_Health;

    //X 축 컨트롤, Z 축 컨트롤이 각각 눌렸는가.
    bool XPress = false;
    bool ZPress = false;

    //사다리
    int tempY = 0;

    enum PlayerDir { Side, Front, Back };
    PlayerDir playerDir;

    //현재 걷는 동작을 할 수 있는 상태인가? (경사가 어느정도 이상이면 걷는 동작이 불가하다)
    bool b_CanWalk = true;
    //현재 사다리 등을 탈 수 있는 상태인가?
    bool b_canClimb = false;
    //현재 사다리 등을 탄 상태인가?
    bool b_Climb = false;
    bool b_Falling = false;
    //공중에 있는 상태인가?
    bool b_Air = false;
    //총을 사용하는 상태인가? : 근접 공격과 구분하기 위함이다.
    bool b_UseGun = false;

    //죽었는지 체크
    bool b_Dead = false;
    bool b_DeadCounterActive = false;

    //너무 높은 곳에서 떨어졌는지 체크
    bool b_tooHigh = false;


    float spriteLocalScale = 1;
    

    CapsuleCollider col;


    Ray SlopeRay1;
    Ray[] GroundRay = new Ray[4];

    Ray[] MoveRayX = new Ray[2];
    Ray[] MoveRayZ = new Ray[2];

    RaycastHit SlopeHit1;
    RaycastHit GroundHit;
    RaycastHit[] MoveHitX = new RaycastHit[2];
    RaycastHit[] MoveHitZ = new RaycastHit[2];
    

    Transform SpriteObj;
    CameraScript camScript;
    Animator anim;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        SpriteObj = transform.Find("Sprite");
        anim = SpriteObj.GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        playerDir = PlayerDir.Back;
        spriteLocalScale = SpriteObj.localScale.x;
        cur_Health = f_Health;
    }
	
	// Update is called once per frame
	void Update () {
        if (b_CanControl)
        {
            if (f_Health <= 0 || b_Dead)
            {
                Debug.Log("Player Dead");
                b_Dead = true;
                anim.Play("Dead");
                if (!b_DeadCounterActive)
                {
                    StartCoroutine(DeadCounter());
                }
            }
            else
            {
                MoveRayControl();
                AnimationControl();
            }
        }

    }


    //인터렉티브 함수
    void InteractiveFunc()
    {

    }

    //이동과 RayCast 관련 함수
    void MoveRayControl()
    {
        tempX = 0;
        tempZ = 0;
        tempY = 0;

        //Slope 검사

        Vector3 SlopeRayPoint1;

        SlopeRayPoint1 = new Vector3(col.bounds.center.x, col.bounds.center.y - col.height/2 + 0.25f, col.bounds.center.z);
        
        int GroundMask = 1 << LayerMask.NameToLayer("Ground");

        


        SlopeRay1 = new Ray(SlopeRayPoint1, Vector3.down);
        GroundRay[0] = new Ray(SlopeRayPoint1 + new Vector3(col.radius - 0.05f, 0), Vector3.down);
        GroundRay[1] = new Ray(SlopeRayPoint1 - new Vector3(col.radius + 0.05f, 0), Vector3.down);
        GroundRay[2] = new Ray(SlopeRayPoint1 + new Vector3(0, 0, col.radius - 0.05f), Vector3.down);
        GroundRay[3] = new Ray(SlopeRayPoint1 - new Vector3(0, 0, col.radius + 0.05f), Vector3.down);

        Debug.DrawRay(SlopeRayPoint1 + new Vector3(col.radius - 0.05f, 0), Vector3.down, Color.red);
        Debug.DrawRay(SlopeRayPoint1 - new Vector3(col.radius + 0.05f, 0), Vector3.down, Color.red);
        Debug.DrawRay(SlopeRayPoint1 + new Vector3(0, 0, col.radius - 0.05f), Vector3.down, Color.red);
        Debug.DrawRay(SlopeRayPoint1 - new Vector3(0, 0, col.radius + 0.05f), Vector3.down, Color.red);

        Debug.DrawRay(SlopeRayPoint1, Vector3.down, Color.red);

        if (Physics.Raycast(SlopeRay1, out SlopeHit1, 0.35f, GroundMask))
        {
            if (Vector3.Dot(Vector3.down, SlopeHit1.normal) > -0.8f)
            {

                Debug.Log("Slope High");
                rigid.velocity -= new Vector3(0, 5, 0);
            }
            if (b_tooHigh)
            {
                b_Dead = true;
            }
        }
        if(Physics.Raycast(GroundRay[0], out GroundHit, 0.35f, GroundMask) || 
           Physics.Raycast(GroundRay[1], out GroundHit, 0.35f, GroundMask) || 
           Physics.Raycast(GroundRay[2], out GroundHit, 0.35f, GroundMask) || 
           Physics.Raycast(GroundRay[3], out GroundHit, 0.35f, GroundMask))
        {
            b_Air = false;
        }else
        {
 
            b_Air = true;

        }


        if (b_Climb)
        {
            b_Air = false;
        }

        //캐릭터이동
        Vector3 CamPosVector = Camera.main.transform.position;
        //외적 구하기
        Vector3 getXAngle = Vector3.Normalize(Vector3.Cross(Vector3.up, Camera.main.transform.forward));
        Vector3 getZAngle = Camera.main.transform.forward;


        Vector3 StartPoint = new Vector3(col.bounds.center.x, col.bounds.center.y, col.bounds.center.z);


        Vector3 temp1 = StartPoint + new Vector3(0, col.height / 2, 0);
        Vector3 temp2 = StartPoint - new Vector3(0, col.height / 2, 0);



        //사다리에서 탄 상태와 타지 않은 상태를 분류한다.
        if (!b_Climb)
        {
            tempY = 0;
            rigid.useGravity = true;

            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !ZPress)
            {
                XPress = true;
            }
            else
            {
                XPress = false;
            }

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && !XPress)
            {
                ZPress = true;
            }
            else
            {
                ZPress = false;
            }

            if (XPress)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    tempX = -1;

                }
                if (Input.GetKey(KeyCode.D))
                {
                    tempX = 1;

                }
            }

            if (ZPress)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    tempZ = 1;

                }
                if (Input.GetKey(KeyCode.S))
                {
                    tempZ = -1;

                }
            }



            if (b_Air)
            {
                MoveRayX[0] = new Ray(temp1, tempX * getXAngle);
                MoveRayX[1] = new Ray(temp2, tempX * getXAngle);
                MoveRayZ[0] = new Ray(temp1, tempZ * getZAngle);
                MoveRayZ[1] = new Ray(temp2, tempZ * getZAngle);
                if (Physics.Raycast(MoveRayX[0], out MoveHitX[0], col.radius + 0.1f, GroundMask) || Physics.Raycast(MoveRayX[1], out MoveHitX[1], col.radius + 0.1f, GroundMask))
                {
                    tempX = 0;
                }
                if (Physics.Raycast(MoveRayZ[0], out MoveHitZ[0], col.radius + 0.1f, GroundMask) || Physics.Raycast(MoveRayZ[1], out MoveHitZ[1], col.radius + 0.1f, GroundMask))
                {
                    tempZ = 0;
                }

                if (rigid.velocity.y <= -f_DeathHeight)
                {
                    b_tooHigh = true;
                }

            }



            float tempSpeed = speed;

            rigid.velocity = Vector3.Normalize(new Vector3((getXAngle.x * tempX + getZAngle.x * tempZ), 0, (getXAngle.z * tempX + getZAngle.z * tempZ))) * tempSpeed + new Vector3(0, rigid.velocity.y, 0);
        }
        else
        {
            rigid.useGravity = false;
            if (Input.GetKey(KeyCode.W))
            {

                tempY = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                tempY = -1;
            }

            rigid.velocity = new Vector3(0, tempY * speed, 0);

        }

        if (Input.GetKeyDown(KeyCode.Space) && !b_Air)
        {
            rigid.velocity += new Vector3(0, f_JumpHeight - tempY * speed);
            if (b_Climb)
            {
                b_Climb = false;
                transform.parent = null;
            }
        }



    }

    //애니메이션 컨트롤 함수
    void AnimationControl()
    {

        SpriteObj.transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        if(tempX > 0)
        {
            SpriteObj.transform.localScale = new Vector3(-spriteLocalScale, spriteLocalScale, spriteLocalScale);
        }
        if(tempX < 0)
        {
            SpriteObj.transform.localScale = new Vector3(spriteLocalScale, spriteLocalScale, spriteLocalScale);
        }
        if(tempZ > 0)
        {
            b_isBack = true;
            playerDir = PlayerDir.Back;
        }
        if(tempZ < 0)
        {
            b_isBack = false;
            playerDir = PlayerDir.Front;
        }
        if(tempZ == 0)
        {
            playerDir = PlayerDir.Side;
        }
         

        if (b_Air)
        {
            if (rigid.velocity.y > 0)
            {
                anim.Play("Jump_Up");
            }
            else
            {
                anim.Play("Jump_Down");
            }
        }
        else
        {
            if (rigid.velocity.x != 0 || rigid.velocity.z != 0)
            {
                anim.Play( "Run");
            }
            else
            {
                //string temp = "Front";
                //if (b_isBack)
                //{
                //    temp = "Back";
                //}
  
                anim.Play( "Idle");
            }
        }
        

    }

    //Damage 함수
    public void TakeDamage(float damage)
    {
        cur_Health -= damage;
    }


    IEnumerator DeadCounter()
    {
        b_DeadCounterActive = true;



        yield return new WaitForSeconds(3f);
        b_tooHigh = false;
        b_DeadCounterActive = false;
        b_Dead = false;
        transform.position = GeneralManager.instance.Respawn.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Grab"))
        {
            Debug.Log("Can Climb");

            if (!b_Climb)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    b_Climb = true;
                    Collider tempCol = other.GetComponent<Collider>();
                    Vector3 tempVec = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                    Debug.Log(other.transform.forward);
                    tempVec += other.transform.forward * col.radius;
                    transform.position = tempVec;
                    transform.parent = other.transform;
                }
                
            }else
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    b_Climb = false;
                    transform.parent = null;
                }
            }
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("EnemyBody"))
        {
            if(other.gameObject.GetComponent<EnemyScript>().enemyState == EnemyScript.EnemyState.Idle)
            {
                Debug.Log("Its Killing Time");
                if (Input.GetKeyDown(KeyCode.F) && !b_UseGun)
                {
                    float hp = other.gameObject.GetComponent<EnemyScript>().f_Health;
                    other.gameObject.GetComponent<EnemyScript>().TakeDamage(hp);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Grab"))
        {
            Debug.Log("Can't Clmib");
            if (b_Climb)
            {
                b_Climb = false;
                transform.parent = null;
            }

        }
    }

}
