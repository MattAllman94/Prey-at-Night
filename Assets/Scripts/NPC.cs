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
    public enum State
    {
        Idle, Patrol, Detect, Hunt
    }

    public EnemyType myType;

    NavMeshAgent agent;
    int currentWaypoint;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        switch (myType)
        {
            case EnemyType.Civilian:
                currentWaypoint = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
                agent.SetDestination(_NPC.civilianWaypoints[currentWaypoint].transform.position);
                break;
            case EnemyType.Criminal:
                currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count - 1);
                agent.SetDestination(_NPC.criminalWaypoints[currentWaypoint].transform.position);
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
                CriminalMovement();
                break;
            case EnemyType.Monster:
                MonsterMovement();
                break;

        }
    }

    #region Movement

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

    public void MonsterMovement()
    {

    }

    #endregion
}
