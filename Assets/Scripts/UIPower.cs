using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UIPower : Prey
{
    public Powers myPower;
    Power power;

    private void Start()
    {
        power = _PM.allPowers.Find(x => x.power == myPower); // Finds the power in allPowers list that is equal to myPower
        // set power icon to power.icon
        // Logic relating to status
    }

    public void Selected()
    {
        _UI.powerName.text = power.GetPowerName();
        _UI.powerDesc.text = power.description;
    }

}