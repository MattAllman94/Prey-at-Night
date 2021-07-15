using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : Prey
{

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Civilian"))
        {
            other.GetComponent<Civilian>().isDraining = true;
            _P.civilianScript = other.GetComponent<Civilian>();
            _P.DrainCivilian();


            //Debug.Log(other.GetComponent<Civilian>().health);
        }
        if (other.CompareTag("Criminal"))
        {
            other.GetComponent<Criminal>().isDraining = true;
            _P.criminalScript = other.GetComponent<Criminal>();
            _P.DrainCriminal();


            //Debug.Log(other.GetComponent<Criminal>().health);
        }

    }
}
