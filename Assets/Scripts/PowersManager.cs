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
        // < INPUT > // 

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


        if (Input.GetKey(KeyCode.Alpha1) && activePower1.damageType == DamageType.DOT) // HOLD POWER 1 
        {
            if (activePower1.power != Powers.NoPower)
                UsePower(activePower1.power);
        }

        if (Input.GetKey(KeyCode.Alpha2) && activePower2.damageType == DamageType.DOT) // HOLD POWER 2 
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

                // use blood
                // damage enemy
                // add health
                // add to bloodrain use counter 
                
                //_GM.currentBlood -= Powers.BloodDrain bloodcost

                print("USED BLOODDRAIN");
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

public enum DamageType
{
    Instant,
    DOT,
}

[System.Serializable]
public class Power 
{
    
    public Powers power;

    public DamageType damageType;

    public PowerStatus powerStatus;

    public CorruptionLevel myRequirement;

    public int activeSlot = 0;

    public int unlockCost;

    public string description;

    public float cooldown;
    public float damage;
    public float range;

    public float bloodCost;

    public Sprite icon;

    public AudioClip castSound;

    // my VFX

    public AnimationClip playerAnimation;

    public AnimationClip enemyAnimation;

    public string GetPowerName()
    {
        return power.ToString();
    }
}