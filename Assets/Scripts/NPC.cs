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



    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
        int currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
        agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);

        float dist = Vector3.Distance(transform.position, _NPC.civilianWaypoints[currentWaypoint].transform.position);
        if (dist <0.1f)
        {
            currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
            agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
        }
    }

    #endregion
}
