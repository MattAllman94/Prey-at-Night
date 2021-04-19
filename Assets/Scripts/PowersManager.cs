using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersManager : Singleton<PowersManager>
{

    [Header ("Powers")]
    public List<Power> allPowers;



}

[System.Serializable]
public class Power 
{
    public enum Powers
    {
        BloodDrain,
        StakeThrow
    }

    public Powers power;

    public float cooldown;
    public float damage;

    public int bloodCost;

    public Sprite icon;

    public AudioClip castSound;

}