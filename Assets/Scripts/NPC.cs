using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    public enum EnemyType
    {
        Civilian, Criminal, Monster, Boss
    }
    public enum State
    {
        Idle, Detect, Attack, Drained, Flee
    }
public class NPC : Prey
{
    public EnemyType myType;
    public State myState;

    public int currentWaypoint;

    public float health;
    public int damage;

    public GameObject player;

    private void Start()
    {

    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.O)) //Test Spawn
        //{
        //    Spawn();
        //}
    }
    public bool ReachedWaypoint(GameObject _waypoint)
    {
        float dist = Vector3.Distance(transform.position, _waypoint.transform.position);
        return dist <= 1f;
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
            _NPC.totalMonsters += 1;
            _NPC.monsters.Remove(this.gameObject);
        }
        else if(myType == EnemyType.Boss)
        {
            _GM.ChangeGameState(GameState.WONGAME);
            _UI.winPanel.SetActive(true);
        }

        Destroy(this.gameObject);
        _NPC.Spawn();
    }

    public void TakeDamage(float _damage, bool _dot = false)
    {
        health -= _dot ? _damage * Time.deltaTime : _damage;
    }
}