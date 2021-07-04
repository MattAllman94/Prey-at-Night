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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        health = 200f;
        player = FindObjectOfType<PlayerController>().gameObject;
    }


    void Update()
    {
        MonsterMovement();

        timer += Time.deltaTime;
    }

    public void MonsterMovement()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (myState)
        {
            case State.Idle:
                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
                break;
            case State.Detect:
                agent.SetDestination(transform.position);
                transform.LookAt(player.transform.position);
                undetectTimer -= Time.deltaTime;
                if (undetectTimer <= 0)
                {
                    if (distToPlayer <= detectDistance)
                    {
                        myState = State.Attack;
                        undetectTimer = 5;
                    }
                    else
                    {
                        myState = State.Idle;
                    }
                }
                break;
            case State.Attack:
                agent.SetDestination(player.transform.position);
                if (distToPlayer > chaseDistance)
                {
                    myState = State.Detect;
                }
                break;
        }

        if (distToPlayer <= detectDistance)
        {
            if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (myState != State.Attack)
                    {
                        myState = State.Detect;
                    }
                }
            }
        }
        else
        {
            myState = State.Idle;
        }

        if (distToPlayer >= chaseDistance)
        {
            if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    myState = State.Attack;
                }
            }
        }
        else
        {
            myState = State.Idle;
        }
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
