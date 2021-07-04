using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class UIPower : Prey
{
    public Powers myPower;
    public Camera uiCamera;
    public Transform borderTransform;
    Power power;
    
    private void Start()
    {
        power = _PM.allPowers.Find(x => x.power == myPower); // Finds the power in allPowers list that is equal to myPower
    }

    Vector3 GetMousePos()
    {
        var mousePos = uiCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }  

    public void BeginDrag()
    {
        if (power.powerStatus == PowerStatus.Purchased || power.powerStatus == PowerStatus.Active)
        {
            Selected();
            transform.position = GetMousePos();
        }                   
    }

    public void Drag()
    {
        if (power.powerStatus == PowerStatus.Purchased || power.powerStatus == PowerStatus.Active)
        {
            transform.position = GetMousePos();
        }           
    }

    public void EndDrag()
    {
        RaycastHit2D hit = (Physics2D.Raycast(transform.position, -Vector2.up));

        if (hit.collider != null)
        {
            if (power.powerStatus == PowerStatus.Purchased || power.powerStatus == PowerStatus.Active)
            {               
                if (hit.collider.gameObject == _UI.equipSlot1)
                {
                    _UI.EquipPower("1");
                    ResetPosition();
                }
                else if (hit.collider.gameObject == _UI.equipSlot2)
                {
                    _UI.EquipPower("2");
                    ResetPosition();
                }
            }
        }  
        else
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = borderTransform.position;
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
