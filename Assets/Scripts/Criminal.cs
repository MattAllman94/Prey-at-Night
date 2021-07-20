using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Criminal : NPC
{
    NavMeshAgent agent;

    public bool inAlley = false;
    public float delay;
    bool attacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;

        ResetNPC();
    }

    public override void ResetNPC()
    {
        //base.ResetNPC();

        currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count);
        agent.SetDestination(_NPC.criminalWaypoints[currentWaypoint].transform.position);

        health = 100f;
        damage = 10;

        ChangeState(State.Idle);
    }

    void Update()
    {
        Response();

        switch(myState)
        {
            case State.Idle:
                if (ReachedWaypoint(_NPC.criminalWaypoints[currentWaypoint]))
                {
                    CriminalMovement();
                }
                break;
            case State.Flee:
                if (ReachedWaypoint(_NPC.civilianSpawn[currentWaypoint]))
                {
                    Die(true);
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

    public void ChangeState(State _state)
    {
        myState = _state;

        switch(myState)
        {
            case State.Idle:
                CriminalMovement();
                agent.isStopped = false;
                break;
            case State.Drained:
                agent.isStopped = true;
                break;
            case State.Attack:
                agent.SetDestination(_P.transform.position);
                agent.isStopped = false;
                break;
            case State.Flee:
                Flee();
                agent.isStopped = false;
                break;
        }
    }

    public void CriminalMovement()
    {
            currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count);
            agent.SetDestination(_NPC.criminalWaypoints[currentWaypoint].transform.position);
    }

    public void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Alley"))
        {
            inAlley = true;
        }
        else
        {
            inAlley = false;
        }
    }

    public void Response()
    {
        if (_GM.corruptionLevel == CorruptionLevel.HIGH)
        {
            if (DistToPlayer <= 4f)
            {
                ChangeState(State.Attack);
            }
        }
        else
        {
            if (inAlley == true && DistToPlayer <= 4f)
            {
                ChangeState(State.Attack);
            }  
        }

        foreach (GameObject i in _NPC.monsters)
        {
            float distToMonster = Vector3.Distance(transform.position, i.transform.position);
            if (distToMonster < 5f)
            {
                ChangeState(State.Flee);
            }
        }

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

    public void Flee()
    {
        currentWaypoint = Random.Range(0, _NPC.civilianSpawn.Count);
        //Debug.Log(currentWaypoint);
        agent.SetDestination(_NPC.civilianSpawn[currentWaypoint].transform.position);
    }
}
