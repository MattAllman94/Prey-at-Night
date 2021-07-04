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
        Idle, Detect, Attack
    }

    public EnemyType myType;
    public State myState;

    public GameObject civilian;
    public GameObject criminal;
    public GameObject monster;

    public int currentWaypoint;

    public float health;
    public int damage;

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
        if(Input.GetKeyDown(KeyCode.O))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        if(_NPC.currentCivilians <= _NPC.totalCivilians - 1)
        {
            Debug.Log("Civilian Spawn");
            int spawnPos = Random.Range(0, _NPC.civilianSpawn.Count - 1);
            GameObject newCivilian = Instantiate(civilian, _NPC.civilianSpawn[spawnPos].transform.position, transform.rotation);
            _NPC.civilians.Add(newCivilian);
            _NPC.currentCivilians += 1;
        }

        if (_NPC.currentCriminals <= _NPC.totalCriminals - 1)
        {
            Debug.Log("Criminal Spawn");
            int spawnPos = Random.Range(0, _NPC.civilianSpawn.Count - 1);
            GameObject newCriminal = Instantiate(criminal, _NPC.civilianSpawn[spawnPos].transform.position, transform.rotation);
            _NPC.criminals.Add(newCriminal);
            _NPC.currentCriminals += 1;
        }

        if (_NPC.currentMonsters <= _NPC.totalMonsters)
        {
            Debug.Log("Monster Spawn");
            int spawnPos = Random.Range(0, _NPC.monsterWaypoints.Count - 1);
            GameObject newMonster = Instantiate(monster, _NPC.monsterWaypoints[spawnPos].transform.position, transform.rotation);
            _NPC.monsters.Add(newMonster);
            _NPC.currentMonsters += 1;
        }
    }

    public void HitPlayer()
    {
        _P.currentHealth -= damage;
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
        Spawn();
    }


}