using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;

public class PowersManager : Singleton<PowersManager>
{

    [Header ("Powers")]
    public List<Power> allPowers;

    public Power activePower1;
    public Power activePower2;

    [Header ("Casting")]
    public Transform castPos;
    

    RaycastHit hit;
    public float rayRange = 100f;

    private void Start()
    {
        //activePower1.power = Powers.NoPower;
        //activePower2.power = Powers.NoPower;
    }

    private void Update()
    {
        if (_GM.gameState != GameState.TITLE)
        {
            #region INPUT // 
            if (Input.GetKeyDown(KeyCode.Alpha1) && activePower1.power != Powers.NoPower & _GM.currentBlood >= activePower1.bloodCost) // USE POWER 1
            {
                if (activePower1.hasCooldown)
                {
                    if (Time.time >= activePower1.nextTimeToCast)
                    {
                        activePower1.nextTimeToCast = Time.time + activePower1.cooldown;
                        UsePower(activePower1);
                    }
                }
                else
                {
                    UsePower(activePower1);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && activePower2.power != Powers.NoPower && _GM.currentBlood >= activePower2.bloodCost) // USE POWER 2
            {
                if (activePower2.hasCooldown)
                {
                    if (Time.time >= activePower2.nextTimeToCast)
                    {
                        activePower2.nextTimeToCast = Time.time + activePower2.cooldown;
                        UsePower(activePower2);
                    }
                }
                else
                {
                    UsePower(activePower2);
                }
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
                    UseStakeThrow(_power);
                    break;
                }
        }
    }

    public void UseBloodDrain(Power _power)
    {
        if(Physics.Raycast(castPos.position, castPos.transform.forward, out hit, GetRange(_power.range))) 
        {
            if(hit.collider.CompareTag("Civilian"))
            {                
                Civilian npcScript = hit.collider.GetComponent<Civilian>();

                float modifier = 0.02f;
                npcScript.ChangeState(State.Drained);
                _GM.ChangeBlood(_power.bloodCost * modifier);              // use blood
                _P.ChangeHealth(_power.bloodCost * (modifier / 2), true);  // add health
                npcScript.TakeDamage(_power.damage);                         // damage enemy        
                _AM.PlayCastSound(_power.castSound);
            }
            else if (hit.collider.CompareTag("Criminal"))
            {
                Criminal npcScript = hit.collider.GetComponent<Criminal>();

                float modifier = 0.02f;
                npcScript.ChangeState(State.Drained);
                _GM.ChangeBlood(_power.bloodCost * modifier);              // use blood
                _P.ChangeHealth(_power.bloodCost * (modifier / 2), true);  // add health
                npcScript.TakeDamage(_power.damage);                           // damage enemy
                _AM.PlayCastSound(_power.castSound);
            }
            else if (hit.collider.CompareTag("Monster"))
            {
                Monster npcScript = hit.collider.GetComponent<Monster>();

                float modifier = 0.02f;
                _GM.ChangeBlood(_power.bloodCost * modifier);              // use blood
                _P.ChangeHealth(_power.bloodCost * (modifier / 2), true);  // add health
                npcScript.TakeDamage(_power.damage);                           // damage enemy
                _AM.PlayCastSound(_power.castSound);
            }
        }
    }

    public void UseStakeThrow(Power _power)
    {      
        GameObject stake = Instantiate(_power.model, castPos.position, castPos.rotation);    // spawn stake
        stake.GetComponent<Stake>().damage = _power.damage;
        Destroy(stake, 10f);    
        // Stake script does the rest 
        _GM.ChangeBlood(_power.bloodCost);                                                   // use blood

        _AM.PlayCastSound(_power.castSound);
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
    StakeThrow,
    ShadowStrike,
    Hypnosis,
    BloodVision,
    ExplosiveBlood,
    CloakedInShadow,
    CreaturesStare,
    ShadowRush,
    BloodLust
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
    public bool hasCooldown = true;
    public float cooldown;
    public float nextTimeToCast; // keep as 0
    public float damage;
    public Range range;  
    public float bloodCost;
    public GameObject model;
    public Sprite icon;
    public AudioClip castSound;
    public AnimationClip playerAnimation;
    public AnimationClip enemyAnimation;

    /*
    public void PlayMyEffect()
    {
        AudioManager.INSTANCE.PlaySFX(castSound);
    }*/

    public string GetPowerName()
    {
        return power.ToString();
    }
}