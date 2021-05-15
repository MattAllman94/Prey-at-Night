using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Prey
{
    public enum EnemyType
    {
        Civilian, Criminal, Monster
    }

    public EnemyType myType;

    NavMeshAgent agent;
    int currentWaypoint;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        switch(myType)
        {
            case EnemyType.Civilian:
                currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count);
                agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
                break;
            case EnemyType.Criminal:
                break;
            case EnemyType.Monster:
                break;
        }
    }

    void Update()
    {
        switch(myType)
        {
            case EnemyType.Civilian:
                CivilianMovement();
                break;
            case EnemyType.Criminal:
                break;
            case EnemyType.Monster:
                break;

        }
    }

    #region Movement

    public void CivilianMovement()
    {
        float dist = Vector3.Distance(transform.position, _NPC.civilianWaypoints[currentWaypoint].transform.position);
        if (dist <= 0.1f)
        {
            currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count);
            agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
            CivilianMovement();
        }
    }

    #endregion
}
