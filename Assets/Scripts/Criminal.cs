using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Criminal : NPC
{
    NavMeshAgent agent;
    
    public bool inAlley = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
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
                Attack();
            }
        }
        else
        {
            if (inAlley == true && distToPlayer <= 4f)
            {
                Attack();
            }
            else
            {
                CriminalMovement();
            }    
        }

        foreach (GameObject i in _NPC.monsters)
        {
            float distToMonster = Vector3.Distance(transform.position, i.transform.position);
            if (distToMonster < 5f)
            {
                Flee();
            }
        }

    }

    public void Attack()
    {

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
