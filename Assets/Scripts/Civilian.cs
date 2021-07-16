using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : NPC
{
    NavMeshAgent agent;
    public GameObject male;
    public GameObject female;

    public bool isDraining;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;

        currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
        agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
        
        health = 100f;

        ChangeState(State.Idle);

        int rnd = Random.Range(0, 2);
        if(rnd == 0)
        {
            male.SetActive(true);
            female.SetActive(false);
        }
        else if(rnd == 1)
        {
            male.SetActive(false);
            female.SetActive(true);
        }
        //Debug.Log(rnd);
    }

    void Update()
    {     
        Response();

        if(ReachedWaypoint(_NPC.civilianWaypoints[currentWaypoint]))
        {
            CivilianMovement();
        }
        if (myState == State.Flee)
        {
            if (ReachedWaypoint(_NPC.civilianSpawn[currentWaypoint]))
            {
                //Debug.Log(ReachedWaypoint(_NPC.civilianSpawn[currentWaypoint]));
                Die(true);
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
                CivilianMovement();
                agent.isStopped = false;
                break;
            case State.Drained:
                agent.isStopped = true;
                break;
            case State.Flee:
                Flee();
                agent.isStopped = false;
                break;
        }
    }

    public void CivilianMovement()
    {
        currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
        agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
    }

    public void Response()
    {
        if (myState == State.Flee)
            return;

        foreach(GameObject i in _NPC.monsters)
        {
            float distToMonster = Vector3.Distance(transform.position, i.transform.position);
            if(distToMonster < 5f)
            {
                myState = State.Flee;
            }
        }

        if(_GM.currentCorruption >= 50f)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if(distToPlayer <= 4f)
            {
                myState = State.Flee;
            }
        }
    }

    public void Flee()
    {
        //Debug.Log("Flee");
        currentWaypoint = Random.Range(0, _NPC.civilianSpawn.Count - 1);
        agent.SetDestination(_NPC.civilianSpawn[currentWaypoint].transform.position);

    }

    
}
