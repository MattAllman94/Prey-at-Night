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
    public Civilian civilianScript;

    private void Start()
    {

    }

    void Update()
    {
        if(health <= 0)
        {
            Die();
        }

        //Debug.Log(health);
        if(Input.GetKeyDown(KeyCode.C))
        {
            health -= 10;
            civilianScript.Flee();
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
        Debug.Log("Die");
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