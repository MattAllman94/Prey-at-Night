using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : Prey
{
    public NPC npc;

    private void OnTriggerEnter(Collider other)
    {
        npc.HitPlayer();
    }
}
