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
    bool attacking = false;

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

        switch (myState)
        {
            case State.Flee:
                if (ReachedWaypoint(waypoint))
                {
                    ChangeState(State.Idle);
                }
                break;
            case State.Attack:
                //Debug.Log(DistToPlayer);
                if (DistToPlayer < 1.5f && !attacking)
                {
                    //Debug.Log("Reached Player");
                    StartCoroutine(Attack());
                }
                if (DistToPlayer > 5)
                {
                    ChangeState(State.Flee);
                }
                break;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(waypoint.transform.position, wanderRadius);
    //}

    public void ChangeState(State _state)
    {
        myState = _state;

        switch(myState)
        {
            case State.Idle:
                MonsterMovement();
                break;
            case State.Attack:
                agent.SetDestination(_P.transform.position);
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
            Vector3 newPos = RandomNavSphere(waypoint.transform.position, wanderRadius, -1);
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

    IEnumerator Attack()
    {
        //Debug.Log("Is Attacking");
        attacking = true;
        //Debug.Log("Attack");
        _P.ChangeHealth(damage, false);
        yield return new WaitForSeconds(delay);
        attacking = false;
    }

    public void MonsterFlee()
    {
        agent.SetDestination(waypoint.transform.position);
        //.Log("Flee");
    }
}
