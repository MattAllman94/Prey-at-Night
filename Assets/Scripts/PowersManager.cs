using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class PowersManager : Singleton<PowersManager>
{

    [Header ("Powers")]
    public List<Power> allPowers;

    public GameObject bloodDrainParticle;

    public Power activePower1 = null;
    public Power activePower2 = null;

    [Header ("Casting")]
    public Transform castPos;

    RaycastHit hit;
    public int rayRange = 100;

    private void Start()
    {
        activePower1.power = Powers.NoPower;
        activePower2.power = Powers.NoPower;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) // USE POWER 1
        {
            if (activePower1.power != Powers.NoPower)
            UsePower(activePower1.power);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // USE POWER 2
        {
            if (activePower2.power != Powers.NoPower)
            UsePower(activePower2.power);
        }
    }

    
    public void UsePower(Powers _power)
    {
        switch (_power)
        {
            case (Powers.BloodDrain):
                {
                    UseBloodrain();
                    break;
                }
            case (Powers.StakeThrow):
                {
                    UseStakeThrow();
                    break;
                }
        }
    }

    public void UseBloodrain()
    {
        if(Physics.Raycast(castPos.position, castPos.transform.forward, out hit, rayRange))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                //GameObject bloodP = Instantiate(bloodDrainParticle, hit.point, Quaternion.identity);
                
                print("Hey!");
            }
        }
    }

    public void UseStakeThrow()
    {
        print("used STAKETHROW!");
    }
    
}

public enum Powers
{
    NoPower,
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

    public int activeSlot = 0;

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