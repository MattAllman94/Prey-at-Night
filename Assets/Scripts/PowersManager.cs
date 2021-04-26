using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersManager : Singleton<PowersManager>
{

    [Header ("Powers")]
    public List<Power> allPowers;



}

public enum Powers
{
    BloodDrain,
    StakeThrow
}

[System.Serializable]
public class Power 
{
    

    public enum PowerStatus
    {
        Locked,
        Unlocked,
        Purchased,
        Active
    }

    public Powers power;

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