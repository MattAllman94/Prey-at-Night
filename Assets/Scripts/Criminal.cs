using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Criminal : NPC
{
    NavMeshAgent agent;
    bool isFleeing = false;

    public bool inAlley = false;
    public float delay;

    public bool isDraining;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;

        currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count - 1);
        agent.SetDestination(_NPC.criminalWaypoints[currentWaypoint].transform.position);
        
        health = 100f;
        damage = 10;

        ChangeState(State.Idle);
    }

    void Update()
    {
        Response();

        if(ReachedWaypoint(_NPC.criminalWaypoints[currentWaypoint]))
        {
            CriminalMovement();
        }
        if(myState == State.Flee)
        {
            if(ReachedWaypoint(_NPC.civilianSpawn[currentWaypoint]))
            {
                Die(true);
            }
        }

        if (health <= 0)
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
                CriminalMovement();
                agent.isStopped = false;
                break;
            case State.Drained:
                agent.isStopped = true;
                break;
            case State.Attack:
                StartCoroutine("Attack");
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
            currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count - 1);
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
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (_GM.currentCorruption >= 50f)
        {
            if (distToPlayer <= 4f)
            {
                myState = State.Attack;
            }
        }
        else
        {
            if (inAlley == true && distToPlayer <= 4f)
            {
                myState = State.Attack;
            }  
        }

        foreach (GameObject i in _NPC.monsters)
        {
            float distToMonster = Vector3.Distance(transform.position, i.transform.position);
            if (distToMonster < 5f)
            {
                myState = State.Flee;
            }
        }

    }

    IEnumerator Attack()
    {
        Debug.Log("Is Attacking");
        
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        agent.SetDestination(player.transform.position);
        
        if (distToPlayer < 0.5f)
        {
            Debug.Log("Attack");
            _P.ChangeHealth(damage, false);
            yield return new WaitForSeconds(delay);
            StartCoroutine("Attack");
        }
    }

    public void Flee()
    {
        currentWaypoint = Random.Range(0, _NPC.civilianSpawn.Count - 1);
        agent.SetDestination(_NPC.civilianSpawn[currentWaypoint].transform.position);
    }
}
