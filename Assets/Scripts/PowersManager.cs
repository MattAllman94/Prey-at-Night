using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersManager : Singleton<PowersManager>
{

    [Header ("Powers")]
    public List<Power> allPowers;

    public Power activePower1 = null;
    public Power activePower2 = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) // USE POWER 1
        {
            print(activePower1.power.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // USE POWER 2
        {
            print(activePower2.power.ToString());
        }
    }
}

public enum Powers
{
    BloodDrain,
    StakeThrow
}

public enum PowerStatus
{
    Locked,
    Unlocked,
    Purchased,
    Active
}

[System.Serializable]
public class Power 
{
    
    public Powers power;

    public PowerStatus powerStatus;

    public CorruptionLevel myRequirement;

    public int unlockCost;

    public string description;

    public float cooldown;
    public float damage;

    public int bloodCost;

    public Sprite icon;

    public AudioClip castSound;

    public string GetPowerName()
    {
        return power.ToString();
    }
}