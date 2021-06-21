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
    NPC npcScript;

    RaycastHit hit;
    public float rayRange = 100f;

    private void Start()
    {
        activePower1.power = Powers.NoPower;
        activePower2.power = Powers.NoPower;
    }

    private void Update()
    {
        #region INPUT // 
        if (Input.GetKeyDown(KeyCode.Alpha1) && _GM.currentBlood >= activePower1.bloodCost) // USE POWER 1
        {
            if (activePower1.power != Powers.NoPower)
            UsePower(activePower1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && _GM.currentBlood >= activePower2.bloodCost) // USE POWER 2
        {
            if (activePower2.power != Powers.NoPower)
                UsePower(activePower2);
        }
       


        if (Input.GetKey(KeyCode.Alpha1) && _GM.currentBlood >= activePower1.bloodCost && activePower1.damageType == DamageType.DOT) // HOLD POWER 1 
        {
            if (activePower1.power != Powers.NoPower)
                UsePower(activePower1);
        }

        if (Input.GetKey(KeyCode.Alpha2) && _GM.currentBlood >= activePower2.bloodCost && activePower2.damageType == DamageType.DOT) // HOLD POWER 2 
        {
            if (activePower2.power != Powers.NoPower)
                UsePower(activePower2);         
        }
        #endregion
    }


    public void UsePower(Power _power)
    {
        switch (_power.power)
        {
            case (Powers.BloodDrain):
                {
                    UseBloodDrain(_power);
                    break;
                }
            case (Powers.StakeThrow):
                {
                    UseStakeThrow();
                    break;
                }
        }
    }

    public void UseBloodDrain(Power _power)
    {
        if(Physics.Raycast(castPos.position, castPos.transform.forward, out hit, GetRange(_power.range))) //_power.range))
        {
            if(hit.collider.CompareTag("NPC"))
            {
                float modifier = 0.02f;
                npcScript = hit.collider.gameObject.GetComponent<NPC>();   // get script off npc hit 

                _GM.ChangeBlood(_power.bloodCost * modifier);              // use blood
                npcScript.health -= _power.damage;                         // damage enemy
                _P.ChangeHealth(_power.bloodCost * (modifier / 2), true);  // add health
            }
        }
    }

    public void UseStakeThrow()
    {
        print("used STAKETHROW!");
    }

    float GetRange(Range _range)
    {
        switch (_range)
        {
            case Range.Short:
                return 5f;
            case Range.Medium:
                return 10f;
            case Range.Long:
                return 15f;
            default:
                return 10f;
        }
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

public enum Range
{
    Short,
    Medium,
    Long
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
    public Range range;  
    public float bloodCost;
    public Sprite icon;
    public AudioClip castSound;
    public AnimationClip playerAnimation;
    public AnimationClip enemyAnimation;

    public void PlayMyEffect()
    {
        AudioManager.INSTANCE.PlaySFX(castSound);
    }

    public string GetPowerName()
    {
        return power.ToString();
    }
}