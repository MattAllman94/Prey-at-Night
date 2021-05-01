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
        _UI.powerTextSide.SetActive(true);
        _UI.powerName.text = power.GetPowerName();
        _UI.powerDesc.text = power.description;
        _UI.selectedPower = power;

        switch (power.powerStatus)
        {
            case (Power.PowerStatus.Unlocked):
            {
                    _UI.unlockButton.SetActive(true);
                    _UI.equipPanel.SetActive(false);
                    _UI.unlockText.text = ("UNLOCK COST: " + power.unlockCost);
                    break;
            }

            case (Power.PowerStatus.Locked):
            {
                    _UI.unlockButton.SetActive(true);
                    _UI.equipPanel.SetActive(false);
                    _UI.unlockText.text = ("LOCKED");
                    break;
            }

            case (Power.PowerStatus.Purchased):
            {
                    _UI.unlockButton.SetActive(false);
                    _UI.equipPanel.SetActive(true);
                    break;
            }

            case (Power.PowerStatus.Active):
            {
                    _UI.unlockButton.SetActive(false);
                    _UI.equipPanel.SetActive(true);
                    break;
            }

        }
    }

}
