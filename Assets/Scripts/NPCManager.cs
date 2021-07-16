using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public int currentCivilians;
    public int currentCriminals;
    public int currentMonsters;

    public int totalCivilians = 15;
    public int totalCriminals = 10;
    public int totalMonsters = 5;

    public List<GameObject> civilians;
    public List<GameObject> criminals;
    public List<GameObject> monsters;

    public List<GameObject> civilianWaypoints;
    public List<GameObject> criminalWaypoints;
    public List<GameObject> monsterWaypoints;

    public List<GameObject> civilianSpawn;

    public GameObject civilian;
    public GameObject criminal;
    public GameObject monster;
    public GameObject boss;

    public void Start()
    {
        boss.SetActive(false);
    }

    public void Update()
    {
        CheckForBoss();

        currentCivilians = civilians.Count;
        currentCriminals = criminals.Count;
    }

    void CheckForBoss()
    {
        if(currentMonsters >= 5)
        {
            //Debug.Log("Boss Spawned");
            boss.SetActive(true);
        }
    }

    public void Spawn()
    {
        if (_NPC.currentCivilians <= _NPC.totalCivilians - 1)
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
    }
}
