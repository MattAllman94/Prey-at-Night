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

    public GameObject civilian;
    public GameObject criminal;
    public GameObject monster;

    public int currentWaypoint;

    public float health;

    public GameObject player;


    private void Start()
    {

        switch (myType)
        {
            case EnemyType.Civilian:
                health = 100f;
                break;
            case EnemyType.Criminal:
                health = 100f;
                break;
            case EnemyType.Monster:
                health = 200f;
                break;
        }
    }

    void Update()
    {
        if(health <= 0)
        {
            Die();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            health -= 10;
        }
    }

    public void Spawn()
    {
        if(_NPC.currentCivilians <= _NPC.totalCivilians)
        {
            //Vector3 spawn = Random.Range(0, _NPC.civilianWaypoints.Count - 1);
            //Instantiate(civilian, 
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
        if(myType == EnemyType.Civilian)
        {
            _NPC.totalCivilians -= 1;
            _NPC.civilians.Remove(this.gameObject);
        }
        else if (myType == EnemyType.Criminal)
        {
            _NPC.totalCriminals -= 1;
            _NPC.criminals.Remove(this.gameObject);
        }
        else if (myType == EnemyType.Monster)
        {
            _NPC.totalMonsters -= 1;
            _NPC.monsters.Remove(this.gameObject);
        }
        
    }
}