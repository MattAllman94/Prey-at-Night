using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public int totalCivilians;
    public int totalCriminals;
    public int totalMonsters;

    public List<GameObject> civilians;
    public List<GameObject> criminals;
    public List<GameObject> monsters;

    public List<GameObject> civilianWaypoints;
    public List<GameObject> criminalWaypoints;
    public List<GameObject> monsterWaypoints;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
