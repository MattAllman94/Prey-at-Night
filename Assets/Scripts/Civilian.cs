using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : NPC
{
    NavMeshAgent agent;
    bool isFleeing = false;
    public GameObject male;
    public GameObject female;

    public bool isDraining;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
        agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
        health = 100f;
        player = FindObjectOfType<PlayerController>().gameObject;

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
        Debug.Log(rnd);
    }

    void Update()
    {
        if(!isDraining || !isFleeing)
        {
            CivilianMovement();
        }
        
        Response();
        
    }

    public void CivilianMovement()
    {
        float dist = Vector3.Distance(transform.position, _NPC.civilianWaypoints[currentWaypoint].transform.position);
        if (dist <= 0.1f)
        {
            currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
            agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
            CivilianMovement();
        }
    }

    public void Response()
    {
        
        foreach(GameObject i in _NPC.monsters)
        {
            float distToMonster = Vector3.Distance(transform.position, i.transform.position);
            if(distToMonster < 5f)
            {
                Flee();
            }
        }

        if(_GM.currentCorruption >= 50f)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if(distToPlayer <= 4f)
            {
                Flee();
            }
        }
        else
        {
            CivilianMovement();
        }
    }

    public void Flee()
    {
        isFleeing = true;
        //Debug.Log("Flee");
        currentWaypoint = Random.Range(0, _NPC.civilianSpawn.Count - 1);
        agent.SetDestination(_NPC.civilianSpawn[currentWaypoint].transform.position);

        float distToEscape = Vector3.Distance(transform.position, _NPC.civilianSpawn[currentWaypoint].transform.position);
        //Debug.Log(distToEscape);
        if (distToEscape <= 0.1f)
        {
            Die();
        }
    }

    
}
