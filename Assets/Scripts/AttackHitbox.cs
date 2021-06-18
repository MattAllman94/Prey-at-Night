using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : Prey
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("NPC"))
        {
            _P.hitNPC = col.gameObject.GetComponent<NPC>();
            _P.HitNPC();
        }
    }
}
