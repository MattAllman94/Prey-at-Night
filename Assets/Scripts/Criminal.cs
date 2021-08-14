using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Criminal : NPC
{
    NavMeshAgent agent;

    public bool inAlley = false;
    public float delay;
    bool attacking = false;

    Vector3 lastPosition;
    Transform myTransform;
    public AudioSource footStepSource;
    public AudioSource myAudioSource;

    public Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        anim = GetComponent<Animator>();
        myTransform = transform;
        lastPosition = myTransform.position;
        myAudioSource = GetComponent<AudioSource>();

        ResetNPC();
    }

    public override void ResetNPC()
    {
        //base.ResetNPC();
        StartCoroutine(Reset());

    }

    IEnumerator Reset()
    {
        ChangeState(State.Dying);
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(3);
        transform.position = _NPC.civilianSpawn[Random.Range(0, _NPC.civilianSpawn.Count)].transform.position;
        anim.SetBool("isDead", false);

        currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count);
        agent.SetDestination(_NPC.criminalWaypoints[currentWaypoint].transform.position);

        health = 100f;
        damage = 10;
        agent.speed = 3.5f;

        ChangeState(State.Idle);
    }

    void Update()
    {
        Response();

        switch(myState)
        {
            case State.Idle:
                if (ReachedWaypoint(_NPC.criminalWaypoints[currentWaypoint]))
                {
                    CriminalMovement();
                }
                break;
            case State.Flee:
                if (ReachedWaypoint(_NPC.civilianSpawn[currentWaypoint]))
                {
                    Die(true);
                }
                break;
            case State.Attack:
                //Debug.Log(DistToPlayer);
                if (DistToPlayer < 2f && !attacking)
                {
                    //Debug.Log("Reached Player");
                    StartCoroutine(Attack());
                }
                if (DistToPlayer > 5)
                {
                    ChangeState(State.Flee);
                }
                break;
        }

        if (myTransform.position != lastPosition) // Check if npc is moving
        {
            _AM.PlayFootStep(footStepSource);
        }          
        lastPosition = myTransform.position;
    }

    public void ChangeState(State _state)
    {
        myState = _state;

        switch(myState)
        {
            case State.Idle:
                CriminalMovement();
                agent.isStopped = false;
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isIdle", false);
                break;
            case State.Drained:
                agent.isStopped = true;
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);
                break;
            case State.Attack:
                agent.SetDestination(_P.transform.position);
                agent.isStopped = false;
                break;
            case State.Flee:
                Flee();
                agent.isStopped = false;
                anim.SetBool("isIdle", false);
                anim.SetBool("isRunning", true);
                anim.SetBool("isWalking", false);
                break;
            case State.Dying:
                agent.isStopped = true;
                break;
        }
    }

    public void CriminalMovement()
    {
            currentWaypoint = Random.Range(0, _NPC.criminalWaypoints.Count);
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
        if (_GM.corruptionLevel == CorruptionLevel.HIGH)
        {
            if (DistToPlayer <= 4f)
            {
                ChangeState(State.Attack);
            }
        }
        else
        {
            if (inAlley == true && DistToPlayer <= 4f)
            {
                ChangeState(State.Attack);
            }  
        }

        foreach (GameObject i in _NPC.monsters)
        {
            float distToMonster = Vector3.Distance(transform.position, i.transform.position);
            if (distToMonster < 5f)
            {
                ChangeState(State.Flee);
            }
        }

    }

    IEnumerator Attack()
    {
        //Debug.Log("Is Attacking");
        attacking = true;
        anim.SetTrigger("Attack");
        //Debug.Log("Attack");
        _P.ChangeHealth(damage, false);
        _AM.PlayerAttackSound(false);
        yield return new WaitForSeconds(delay);
        attacking = false;
    }

    public void Flee()
    {
        agent.speed = 5f;
        currentWaypoint = Random.Range(0, _NPC.civilianSpawn.Count);
        //Debug.Log(currentWaypoint);
        agent.SetDestination(_NPC.civilianSpawn[currentWaypoint].transform.position);
    }
}
