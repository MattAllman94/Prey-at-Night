using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : NPC
{

    NavMeshAgent agent;

    private float timer;

    public float wanderRadius;
    public float wanderTimer;
    public float detectDistance;
    public float undetectTimer;
    public float chaseDistance;
    public GameObject waypoint;

    public float delay;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        timer = 0;
        
        health = 200f;
        damage = 20;

        ChangeState(State.Idle);
    }


    void Update()
    {
        timer += Time.deltaTime;
        MonsterReact();

        if(myState == State.Flee)
        {
            if(ReachedWaypoint(waypoint))
            {
                ChangeState(State.Idle);
            }
        }

        if(health <= 0)
        {
            Die(false);
        }
    }

    public void ChangeState(State _state)
    {
        myState = _state;

        switch(myState)
        {
            case State.Idle:
                MonsterMovement();
                break;
            case State.Attack:
                MonsterAttack();
                break;
            case State.Flee:
                MonsterFlee();
                break;
        }
    }

    public void MonsterReact()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distToPlayer <= detectDistance)
        {
            if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    ChangeState(State.Attack);
                }
            }
        }
        else if(myState != State.Flee)
        {
            ChangeState(State.Idle);
        }
    }

    public void MonsterMovement()
    {
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }       
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    //public void MonsterDetect()
    //{
    //    float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
    //    agent.SetDestination(transform.position);
    //    transform.LookAt(player.transform.position);
    //    undetectTimer -= Time.deltaTime;
        
    //    if (undetectTimer <= 0)
    //    {
    //        if (distToPlayer >= detectDistance)
    //        {
    //            ChangeState(State.Idle);
    //            undetectTimer = 5;
    //        }
    //    }

    //    if (distToPlayer <= detectDistance)
    //    {
    //        ChangeState(State.Attack);
    //        undetectTimer = 5;
    //    }
    //}

    public void MonsterAttack()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        agent.SetDestination(player.transform.position);
        Debug.Log("Begin Attack");
        
        if(distToPlayer <= 0.5f)
        {
            StartCoroutine("Attack");
        }
        
        if (distToPlayer > chaseDistance)
        {
            ChangeState(State.Flee);
        }
    }

    IEnumerator Attack()
    {
        _P.ChangeHealth(damage, false);
        Debug.Log("Hit");
        yield return new WaitForSeconds(delay);
        StartCoroutine("Attack");
    }

    public void MonsterFlee()
    {
        agent.SetDestination(waypoint.transform.position);
        //.Log("Flee");
    }
}
