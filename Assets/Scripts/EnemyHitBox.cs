using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : Prey
{


    private void OnTriggerEnter(Collider other)
    {
        _P.currentHealth -= 10;
    }
}
