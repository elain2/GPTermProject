using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {

    public NavMeshAgent agent;
    public float f_Health = 3;
    public Transform[] PatrolPoint;
    int NextPatrolPoint = 0;

    //Enemy의 기본속도(패트롤속도)
    public float enemySpeed = 1;

    //총라인 게임 오브젝트
    LineRenderer GunShotLine;


    //Enemy의 시작 포지션
    Vector3 StartPosition;

    //패트롤은 몇초에 한번씩 수행하는가
    public float f_PatrolRestartTime = 3;
    bool b_Patrol = false;

    
    //Detect 게이지
    [Range(0, 100)]
    public float f_FindGauge = 0;

    //타겟을 찾는 Ray
    Ray DetectRay;
    
    //적의 상태
    public enum EnemyState { Idle, Chase, Attack, Research, Doudt, Dead }
    public EnemyState enemyState;

    //적의 BodyCollider
    public CapsuleCollider Bodycol;

    //Gauge Control 코루틴이 돌아가고 있는가?
    bool b_GaugeCoroutineRun = false;

    //공격 코루틴이 돌아가고 있는가?
    bool b_GunShotCorRun = false;

    //패트롤 시간 설정 코루틴
    bool b_PatrolCorRun = false;

    //Chase 상태에서의 Target
    GameObject Target;

    //Chase 상태에서 플레이어의 마지막 위치
    Vector3 PlayerLastPosition;


    //Chase 상태에서의 Collider
    SphereCollider ChaseCol;

    

	// Use this for initialization
	void Start () {
        Bodycol = GetComponent<CapsuleCollider>();
        
        if(PatrolPoint.Length == 0)
        {
            b_Patrol = false;
        }else
        {
            b_Patrol = true;

        }
        StartPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        enemyState = EnemyState.Idle;
        ChaseCol = GetComponentInChildren<SphereCollider>();
        ChaseCol.enabled = false;
        GunShotLine = transform.Find("Gun").GetComponent<LineRenderer>();
        GunShotLine.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {

        if (f_Health <= 0)
        {
            Debug.Log("EnemyDead");
            agent.ResetPath();
            enemyState = EnemyState.Dead;
        }
        else
        {
            Debug.Log("Current State : " + enemyState);


            //idle
            if (enemyState == EnemyState.Idle)
            {
                agent.speed = enemySpeed;
                IdleBehave();
            }

            //doudt
            if (enemyState == EnemyState.Doudt)
            {
                //doudt 면 그 자리에서 멈춰서 아무것도 하지 않는다.
                agent.isStopped = true;
                if (f_FindGauge >= 100)
                {
                    enemyState = EnemyState.Attack;

                }
            }
            else
            {
                agent.isStopped = false;
            }

            //attack
            if (enemyState == EnemyState.Attack)
            {
                StopCoroutine(GaugeCoroutine());
                b_GaugeCoroutineRun = false;
                agent.ResetPath();
                ChaseCol.enabled = true;

                //Attack Behave의 대부분은 Trigger 코드에서 이루어진다.
            }else
            {
                ChaseCol.enabled = false;
            }

            //chase
            if (enemyState == EnemyState.Chase)
            {
                agent.speed = enemySpeed * 2;
                ChaseBehave();
            }

        }
    }
    void IdleBehave()
    {

        ChaseCol.enabled = false;
        f_FindGauge = 0;
        

        if (b_Patrol)
        {

            Debug.Log("next patrol : " + NextPatrolPoint);

            if(NextPatrolPoint >= PatrolPoint.Length)
            {

                if(((int)transform.position.x != (int)StartPosition.x) || ((int)transform.position.z != (int)StartPosition.z))
                {
                    agent.SetDestination(StartPosition);
                }else
                {
                    if (!b_PatrolCorRun)
                    {
                        StartCoroutine(PatrolCor(0));
                    }
                }
            }else
            {

                if (((int)transform.position.x != (int)PatrolPoint[NextPatrolPoint].position.x) || ((int)transform.position.z != (int)PatrolPoint[NextPatrolPoint].position.z))
                {

                    agent.SetDestination(PatrolPoint[NextPatrolPoint].position);
                }
                else
                {
                    if (!b_PatrolCorRun)
                    {
                        StartCoroutine(PatrolCor(NextPatrolPoint+1));
                    }
                }
            }
        }else
        {
            if (((int)transform.position.x != (int)StartPosition.x) || ((int)transform.position.z != (int)StartPosition.z))
            {
                Debug.Log("Its Not my position");
                agent.SetDestination(StartPosition);
            }else
            {
                agent.ResetPath();
            }
        }
    }
    void ChaseBehave()
    {
        if (!b_GaugeCoroutineRun)
        {
            StartCoroutine(GaugeCoroutine());
        }
        if (f_FindGauge <= 0)
        {
            agent.ResetPath();
            enemyState = EnemyState.Idle;
        }
        agent.SetDestination(PlayerLastPosition);
    }

    public void TakeDamage(float Damage)
    {
        f_Health -= Damage;
    }



    IEnumerator GaugeCoroutine()
    {
        b_GaugeCoroutineRun = true;
        if (enemyState == EnemyState.Doudt)
        {
            f_FindGauge += 5;
            yield return null;
        }else
        {
            f_FindGauge -= 10f;
            yield return new WaitForSeconds(2f);
        }
        yield return null;
        b_GaugeCoroutineRun = false;
    }

    IEnumerator GunShotCor(Vector3 pos)
    {
        b_GunShotCorRun = true;
        GunShotLine.gameObject.SetActive(true);
        GunShotLine.SetPosition(0, GunShotLine.gameObject.transform.position);
        GunShotLine.SetPosition(1, pos);
        yield return new WaitForSeconds(0.1f);
        GunShotLine.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        b_GunShotCorRun = false;
    }

    IEnumerator PatrolCor(int next)
    {
        b_PatrolCorRun = true;
        agent.ResetPath();
        yield return new WaitForSeconds(f_PatrolRestartTime);
        NextPatrolPoint = next;
        b_PatrolCorRun = false;
    }





    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
        {
            CapsuleCollider tempCol = col.GetComponent<CapsuleCollider>();
   

            Vector3 tempDir = Vector3.Normalize(tempCol.transform.TransformPoint(tempCol.center) - transform.TransformPoint(Bodycol.center));
            Ray tempRay = new Ray(transform.TransformPoint(Bodycol.center), tempDir);
            RaycastHit tempHit;
            int tempMask = (1 << LayerMask.NameToLayer("PlayerBody")) + (1 << LayerMask.NameToLayer("Ground"));
            Debug.DrawRay(tempRay.origin, tempRay.direction, Color.blue);

            if (enemyState == EnemyState.Idle)
            {



                if (Physics.Raycast(tempRay, out tempHit, 100, tempMask))
                {
                    if (tempHit.transform.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
                    {
                        Debug.Log("Detect");
                        enemyState = EnemyState.Doudt;
                    }

                }


            }
            else if (enemyState == EnemyState.Doudt)
            {
                if (!b_GaugeCoroutineRun)
                {
                    StartCoroutine(GaugeCoroutine());
                }
            }
            else if (enemyState == EnemyState.Attack)
            {


                if (Physics.Raycast(tempRay, out tempHit, 100, tempMask))
                {

                    Debug.Log("I'll Attack");
                    if (!b_GunShotCorRun)
                    {
                        if(tempHit.transform.position == col.transform.position)
                        {
                            Debug.Log("I Shoot Player");
                            col.gameObject.GetComponent<PlayerPhysics>().TakeDamage(1);
                        }
                        StartCoroutine(GunShotCor(tempHit.point));
                    }
                }
            }
            else if(enemyState == EnemyState.Chase)
            {

                if (Physics.Raycast(tempRay, out tempHit, 100, tempMask))
                {
                    if (tempHit.transform.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
                    {
                        Debug.Log("Found you AGAIN");
                        enemyState = EnemyState.Attack;
                    }

                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
        {
            if (enemyState == EnemyState.Doudt)
            {

                enemyState = EnemyState.Idle;

            }
            if (enemyState == EnemyState.Attack)
            {
                Debug.Log("Lost Target");
                PlayerLastPosition = col.transform.position;
                enemyState = EnemyState.Chase;
            }
        }
    }

}
