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
        //CheckForBoss();

        currentCivilians = civilians.Count;
        currentCriminals = criminals.Count;
    }

    public void CheckForBoss()
    {
        if(currentMonsters >= 5)
        {
            //Debug.Log("Boss Spawned");
            boss.SetActive(true);
            StartCoroutine(_UI.UpdateBossSpawn());
        }
    }
}
