using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : Prey
{
    Civilian civilian;
    //Criminal criminal;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Civilian"))
        {
            civilian = other.GetComponent<Civilian>();
            civilian.ChangeState(State.Drained);
            _P.civilianScript = other.GetComponent<Civilian>();

            _P.DrainCivilian();
        }

        if (other.CompareTag("Criminal"))
        {
            other.GetComponent<Criminal>().isDraining = true;
            _P.criminalScript = other.GetComponent<Criminal>();
            _P.DrainCriminal();
        }

    }

    private void OnDisable()
    {
        if(civilian != null)
        {
            civilian.ChangeState(State.Flee);
        }
        //criminal.ChangeState(State.Attack);
    }
}
