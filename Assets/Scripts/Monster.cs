using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : NPC
{

    NavMeshAgent agent;

    private float timer;

    public float wanderRadius;
    public float wanderTimer;
    public float detectDistance;
    public float undetectTimer;
    public float chaseDistance;
    public GameObject waypoint;

    public float delay;
    bool attacking = false;

    Vector3 lastPosition;
    Transform myTransform;
    public AudioSource footStepSource;

    public Animator anim;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        timer = 0;
        myTransform = transform;
        lastPosition = myTransform.position;

        health = 200f;
        damage = 20;

        ChangeState(State.Idle);
    }


    void Update()
    {
        timer += Time.deltaTime;
        MonsterReact();

        switch (myState)
        {
            case State.Flee:
                if (ReachedWaypoint(waypoint))
                {
                    ChangeState(State.Idle);
                }
                break;
            case State.Attack:
                //Debug.Log(DistToPlayer);
                if (DistToPlayer < 1.8f && !attacking)
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

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(waypoint.transform.position, wanderRadius);
    //}

    public override void ResetNPC()
    {
        base.ResetNPC();

        Destroy(this.gameObject.GetComponent<CapsuleCollider>());
        StartCoroutine(Death());
    }

    public void ChangeState(State _state)
    {
        myState = _state;

        switch(myState)
        {
            case State.Idle:
                MonsterMovement();
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                break;
            case State.Attack:
                agent.SetDestination(_P.transform.position);
                anim.SetBool("isWalking", false);
                break;
            case State.Flee:
                MonsterFlee();
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                break;
        }
    }

    public void MonsterReact()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distToPlayer <= detectDistance)
        {
            if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    ChangeState(State.Attack);
                }
            }
        }
        else if(myState != State.Flee)
        {
            ChangeState(State.Idle);
        }
    }

    public void MonsterMovement()
    {
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(waypoint.transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
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

    IEnumerator Attack()
    {
        //Debug.Log("Is Attacking");
        attacking = true;
        anim.SetBool("isAttacking", true);
        //Debug.Log("Attack");
        _P.ChangeHealth(damage, false);
        yield return new WaitForSeconds(delay);
        attacking = false;
        anim.SetBool("isAttacking", false);
    }

    public void MonsterFlee()
    {
        agent.SetDestination(waypoint.transform.position);
        //.Log("Flee");
    }

    IEnumerator Death()
    {
        agent.isStopped = true;
        anim.SetBool("Died", true);
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
