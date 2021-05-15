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
        Idle, Detect, Hunt
    }

    public EnemyType myType;
    public State myState;

    NavMeshAgent agent;
    int currentWaypoint;

    public GameObject player;
    private float timer;
    public float wanderRadius;
    public float wanderTimer;
    public float detectDistance;
    public float undetectTimer;
    public float chaseDistance;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

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
        switch (myType)
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
        timer += Time.deltaTime;

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
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (myState)
        {
            case State.Idle:
                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
                break;
            case State.Detect:
                agent.SetDestination(transform.position);
                transform.LookAt(player.transform.position);
                undetectTimer -= Time.deltaTime;
                if (undetectTimer <= 0)
                {
                    if (distToPlayer <= detectDistance)
                    {
                        myState = State.Hunt;
                        undetectTimer = 5;
                    }
                    else
                    {
                        myState = State.Idle;
                    }
                }
                break;
            case State.Hunt:
                agent.SetDestination(player.transform.position);
                if (distToPlayer > chaseDistance)
                {
                    myState = State.Detect;
                }
                break;
        }

        if (distToPlayer <= detectDistance)
        {
            if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (myState != State.Hunt)
                    {
                        myState = State.Detect;
                    }
                }
            }
        }
        else
        {
            myState = State.Idle;
        }

        if (distToPlayer >= chaseDistance)
        {
            if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    myState = State.Hunt;
                }
            }
        }
        else
        {
            myState = State.Idle;
        }
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    #endregion
        
}