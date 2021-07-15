using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : Prey
{
   
   private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Civilian"))
        {
            _P.hitNPC = col.GetComponent<Civilian>();
            _P.civilianScript = col.GetComponent<Civilian>();
            _P.HitNPC();
            //Debug.Log("Hit");
        }

        if (col.CompareTag("Criminal"))
        {
            _P.hitNPC = col.GetComponent<Criminal>();
            _P.criminalScript = col.GetComponent<Criminal>();
            _P.HitNPC();
            //Debug.Log("Hit");
        }

        if (col.CompareTag("Monster"))
        {
            _P.hitNPC = col.GetComponent<Monster>();
            _P.monsterScript = col.GetComponent<Monster>();
            _P.HitNPC();
            //Debug.Log("Hit");
        }
    }
}
