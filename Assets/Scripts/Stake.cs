using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stake : Prey
{
    Rigidbody rb;
    CapsuleCollider collider;
    public float thrust = 500f;
    public float damage;
    public AudioSource audioSource;
    public AudioClip npcHitSound;
    public AudioClip enviroHitSound;

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
            npcScript.health -= damage;

            audioSource.clip = npcHitSound;
            audioSource.pitch = Random.Range(0.8f, 1f);
            audioSource.Play();

            if (npcScript.health <= 0)
                npcScript.Die();           
        }
        else if (col.gameObject.CompareTag("Criminal"))
        {
            gameObject.transform.SetParent(col.gameObject.transform);
            Criminal npcScript = col.gameObject.GetComponent<Criminal>();
            npcScript.health -= damage;

            audioSource.clip = npcHitSound;
            audioSource.pitch = Random.Range(0.8f, 1f);
            audioSource.Play();

            if (npcScript.health <= 0)
                npcScript.Die();
        }
        else if (col.gameObject.CompareTag("Monster"))
        {
            gameObject.transform.SetParent(col.gameObject.transform);
            Monster npcScript = col.gameObject.GetComponent<Monster>();
            npcScript.health -= damage;

            audioSource.clip = npcHitSound;
            audioSource.pitch = Random.Range(0.8f, 1f);
            audioSource.Play();

            if (npcScript.health <= 0)
                npcScript.Die();
        }
        else
        {
            audioSource.clip = enviroHitSound;
            audioSource.pitch = Random.Range(0.8f, 1f);
            audioSource.Play();
            // play environment impact sound
        }
    }

}
