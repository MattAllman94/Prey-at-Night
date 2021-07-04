using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Criminal : NPC
{
    NavMeshAgent agent;
    bool isFleeing = false;

    public bool inAlley = false;
    public GameObject hitbox;
    public bool isAttacking = false;
    public float delay;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count - 1);
        agent.SetDestination(_NPC.criminalWaypoints[currentWaypoint].transform.position);
        health = 100f;
        player = FindObjectOfType<PlayerController>().gameObject;
        damage = 10;
    }

    void Update()
    {
        if (!isFleeing)
        {
            CriminalMovement();
        }
        Response();
    }

    public void CriminalMovement()
    {
        float dist = Vector3.Distance(transform.position, _NPC.criminalWaypoints[currentWaypoint].transform.position);
        if (dist <= 0.1f)
        {
            currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count - 1);
            agent.SetDestination(_NPC.criminalWaypoints[currentWaypoint].transform.position);
            CriminalMovement();
        }
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
                StartCoroutine("Attack");
            }
        }
        else
        {
            if (inAlley == true && distToPlayer <= 4f)
            {
                StartCoroutine("Attack");
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

    IEnumerator Attack()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        agent.SetDestination(player.transform.position);
        if (distToPlayer < 0.2f)
        {
            isAttacking = true;
            hitbox.SetActive(true);
            yield return new WaitForSeconds(delay);
            hitbox.SetActive(false);
            isAttacking = false;
        }
    }

    public void Flee()
    {
        isFleeing = true;
        currentWaypoint = Random.Range(0, _NPC.civilianSpawn.Count - 1);
        agent.SetDestination(_NPC.civilianSpawn[currentWaypoint].transform.position);

        float distToEscape = Vector3.Distance(transform.position, _NPC.civilianSpawn[currentWaypoint].transform.position);
        if (distToEscape <= 0.01f)
        {
            Die();
        }
    }
}
