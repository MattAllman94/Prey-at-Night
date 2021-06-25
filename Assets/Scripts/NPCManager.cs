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

    NPC npcScript;

    public List<GameObject> civilians;
    public List<GameObject> criminals;
    public List<GameObject> monsters;

    public List<GameObject> civilianWaypoints;
    public List<GameObject> criminalWaypoints;
    public List<GameObject> monsterWaypoints;

    public List<GameObject> civilianSpawn;
    public List<GameObject> monsterSpawn;

    void Update()
    {
        if(currentCivilians < totalCivilians || currentCriminals < totalCriminals || currentMonsters < totalMonsters)
        {
            //npcScript.Spawn();
        }
    }
}
