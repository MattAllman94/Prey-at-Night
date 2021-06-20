using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : NPC
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
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
        currentWaypoint = Random.Range(0, _NPC.civilianSpawn.Count - 1);
        agent.SetDestination(_NPC.civilianSpawn[currentWaypoint].transform.position);

        float distToEscape = Vector3.Distance(transform.position, _NPC.civilianSpawn[currentWaypoint].transform.position);
        if (distToEscape <= 0.01f)
        {
            Despawn();
        }
    }
}
