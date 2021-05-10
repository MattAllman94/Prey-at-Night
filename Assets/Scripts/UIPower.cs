using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
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
            case (PowerStatus.Unlocked):
            {
                    _UI.unlockButton.SetActive(true);
                    _UI.unlockText.text = ("UNLOCK COST: " + power.unlockCost);
                    _UI.equippedText.text = "";
                    break;
            }

            case (PowerStatus.Locked):
            {
                    if (_GM.corruptionLevel == power.myRequirement || power.myRequirement == CorruptionLevel.NORMAL)
                    {
                        power.powerStatus = PowerStatus.Unlocked;
                        _UI.unlockButton.SetActive(true);
                        _UI.unlockText.text = ("UNLOCK COST: " + power.unlockCost);
                        _UI.equippedText.text = "";
                        break;
                    }
                    else
                    {
                        _UI.unlockButton.SetActive(true);
                        _UI.unlockText.text = ("LOCKED");
                        _UI.equippedText.text = "";
                        break;
                    }
                     
            }

            case (PowerStatus.Purchased):
            {
                    _UI.unlockButton.SetActive(false);
                    _UI.equippedText.text = "";
                    break;
            }

            case (PowerStatus.Active):
            {
                    _UI.unlockButton.SetActive(false);

                    _UI.equippedText.text = ("Equipped: Slot " + power.activeSlot);
                    break;   
            }

        }
    }

}
