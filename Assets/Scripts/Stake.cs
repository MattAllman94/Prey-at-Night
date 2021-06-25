using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stake : Prey
{
    Rigidbody rb;
    CapsuleCollider collider;
    public float thrust = 500f;
    public float damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
 
        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision col)
    {
        rb.isKinematic = true;
        collider.enabled = false;

        if (col.gameObject.CompareTag("NPC"))
        {          
            gameObject.transform.SetParent(col.gameObject.transform);
            NPC npcScript = col.gameObject.GetComponent<NPC>();
            npcScript.health -= damage;       
        }
    }

}
