using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    public enum EnemyType
    {
        Civilian, Criminal, Monster
    }
    public enum State
    {
        Idle, Detect, Attack, Drained, Flee
    }
public class NPC : Prey
{
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
        if(Input.GetKeyDown(KeyCode.O)) //Test Spawn
        {
            Spawn();
        }
    }
    public bool ReachedWaypoint(GameObject _waypoint)
    {
        float dist = Vector3.Distance(transform.position, _waypoint.transform.position);
        return dist <= 1f;
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

        //if (_NPC.currentMonsters <= _NPC.totalMonsters)
        //{
        //    Debug.Log("Monster Spawn");
        //    int spawnPos = Random.Range(0, _NPC.monsterWaypoints.Count - 1);
        //    GameObject newMonster = Instantiate(monster, _NPC.monsterWaypoints[spawnPos].transform.position, transform.rotation);
        //    _NPC.monsters.Add(newMonster);
        //    _NPC.currentMonsters += 1;
        //}
    }

    public void HitPlayer()
    {
        _P.ChangeHealth(damage, false);
    }

    public void Die(bool _despawn = false)
    {
        //Debug.Log("Die");
        if(myType == EnemyType.Civilian)
        {
            _NPC.totalCivilians -= 1;
            _NPC.civilians.Remove(this.gameObject);
            _GM.currentCorruption += _despawn ? 0 : 10;
            _GM.powerPoints += _despawn ? 0 : 1;
        }
        else if (myType == EnemyType.Criminal)
        {
            _NPC.totalCriminals -= 1;
            _NPC.criminals.Remove(this.gameObject);
            _GM.currentCorruption -= _despawn ? 0 : 10;
            _GM.powerPoints += _despawn ? 0 : 1;
        }
        else if (myType == EnemyType.Monster)
        {
            _NPC.totalMonsters -= 1;
            _NPC.monsters.Remove(this.gameObject);
        }

        Destroy(this.gameObject);
        Spawn();
    }

    public void TakeDamage(float _damage, bool _dot = false)
    {

        health -= _dot ? _damage * Time.deltaTime : _damage;

        if (health <= 0)
        {
            Die(false);
        }
    }
}