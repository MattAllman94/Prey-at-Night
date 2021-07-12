using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stake : Prey
{
    Rigidbody rb;
    CapsuleCollider collider;
    public float thrust = 500f;
    public float damage;

    AudioSource audioSource;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
 
        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision col)
    {
        rb.isKinematic = true;
        collider.enabled = false;

        if (col.gameObject.CompareTag("Civilian"))
        {          
            gameObject.transform.SetParent(col.gameObject.transform);
            Civilian npcScript = col.gameObject.GetComponent<Civilian>();
            _AM.PlaySFX(_AM.stakeHitNpc, transform.position);
            npcScript.TakeDamage(damage);
        }
        else if (col.gameObject.CompareTag("Criminal"))
        {
            gameObject.transform.SetParent(col.gameObject.transform);
            Criminal npcScript = col.gameObject.GetComponent<Criminal>();
            _AM.PlaySFX(_AM.stakeHitNpc, transform.position);
            npcScript.TakeDamage(damage);       
        }
        else if (col.gameObject.CompareTag("Monster"))
        {
            gameObject.transform.SetParent(col.gameObject.transform);
            Monster npcScript = col.gameObject.GetComponent<Monster>();
            _AM.PlaySFX(_AM.stakeHitNpc, transform.position);
            npcScript.TakeDamage(damage);
        }
        else
        {
            _AM.PlaySFX(_AM.stakeHitEnviro, transform.position);
            // play environment impact sound
        }
    }

 

}
