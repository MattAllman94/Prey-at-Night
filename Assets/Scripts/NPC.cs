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

    public float DistToPlayer { get { return Vector3.Distance(transform.position, _P.transform.position); } }

    public void Die(bool _despawn = false)
    {
        //Debug.Log("Die");
        if(myType == EnemyType.Civilian)
        {
            _GM.currentCorruption += _despawn ? 0 : 10;
            _GM.powerPoints += _despawn ? 0 : 2;
            _NPC.civiliansKilled += _despawn ? 0 : 1;
            if(_NPC.civiliansKilled == 1 && _despawn == false)
            {
                _PROMPT.ChangeState(PromptState.Four);
            }
        }
        else if (myType == EnemyType.Criminal)
        {
            _GM.currentCorruption -= _despawn ? 0 : 10;
            _GM.powerPoints += _despawn ? 0 : 1;
            _NPC.criminalsKilled += _despawn ? 0 : 1;
            if(_NPC.criminalsKilled == 1 && _despawn == false)
            {
                _PROMPT.ChangeState(PromptState.Five);
            }
        }
        else if (myType == EnemyType.Monster)
        {
            _NPC.monstersKilled += 1;
            _NPC.monsters.Remove(this.gameObject);
            //Destroy(this.gameObject);
            _UI.UpdateMonstersDefeated(_NPC.monstersKilled);
            _NPC.CheckForBoss();
            if(_NPC.monstersKilled == 1)
            {
                _PROMPT.ChangeState(PromptState.Eight);
            }
        }
        else if(myType == EnemyType.Boss)
        {
            _GM.ChangeGameState(GameState.WONGAME);
            _UI.winPanel.SetActive(true);
        }

        //TODO
        //Ensure that waypoint selected is not close/visible to the player
        transform.position = _NPC.civilianSpawn[Random.Range(0, _NPC.civilianSpawn.Count)].transform.position;
        ResetNPC();
    }

    public virtual void ResetNPC()
    {
        Debug.Log("NPC base reset");
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