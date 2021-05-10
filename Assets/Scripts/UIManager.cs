using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class UIManager : Singleton<UIManager>
{
    public GameObject inGamePanel;

    [Header ("Power Tree")]
    public GameObject powerTreePanel;
    public GameObject powerTextSide;
    public GameObject warningPanel;

    public TextMeshProUGUI powerName;
    public TextMeshProUGUI equippedText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI powerDesc;
    public TextMeshProUGUI ppText;

    public GameObject unlockButton;
    public TextMeshProUGUI unlockText;

    public GameObject equipPanel;

    public Power selectedPower;

    int warnSlot = 0;

    private void Start()
    {
        powerTreePanel.SetActive(false);
        powerTextSide.SetActive(false);
        ppText.text = ("PP: " + _GM.powerPoints.ToString());
        
    }


   
    
    public void PurchasePower()
    {
        if (_GM.powerPoints >= selectedPower.unlockCost && selectedPower.powerStatus == PowerStatus.Unlocked)
        {
            _GM.powerPoints -= selectedPower.unlockCost;
            ppText.text = ("PP: " + _GM.powerPoints.ToString());
            selectedPower.powerStatus = PowerStatus.Purchased;
            unlockButton.SetActive(false);
        }
    }

    public void EquipPower(string _EquipKey)
    {
        if(selectedPower.powerStatus == PowerStatus.Purchased || selectedPower.powerStatus == PowerStatus.Active)
        {
            selectedPower.powerStatus = PowerStatus.Active;

            switch (_EquipKey)
            {
                case ("1"):
                    {
                        if (_PM.activePower1 != selectedPower && _PM.activePower1.power != Powers.NoPower) // checks if theres a power on slot 2
                        {
                            //warn Player
                            DisplayWarning(1);             
                        }
                        else
                        {
                            if (_PM.activePower2 == selectedPower) 
                            {
                                _PM.activePower2 = null; // remove selected power assignment from other keys
                            }

                            _PM.activePower1 = selectedPower;
                            selectedPower.activeSlot = 1;
                            equippedText.text = "Equipped: Slot 1";                       
                        }
                        break;
                    }

                case ("2"):
                    {
                        if (_PM.activePower2 != selectedPower && _PM.activePower2.power != Powers.NoPower) // checks if theres a power on slot 2
                        {
                            //warn Player
                            DisplayWarning(2);
                        }
                        else
                        {
                            if (_PM.activePower1 == selectedPower) 
                            {
                                _PM.activePower1 = null; // remove selected power assignment from other keys
                            }

                            _PM.activePower2 = selectedPower;
                            selectedPower.activeSlot = 2;
                            equippedText.text = "Equipped: Slot 2";
                        }
                        break;                     
                    }
            }  
        }
    }

    public void ClearUI()
    {
        powerName.text = "";
        powerDesc.text = "";
        equippedText.text = "";
        unlockButton.SetActive(false);
        selectedPower = null;
    }

    public void DisplayWarning(int _slotNum)
    {
        warningPanel.SetActive(true);
        switch (_slotNum)
        {
            case (1):
                {
                    warnSlot = 1; //< used for overwriting  
                    warningText.text = ("Overwrite " + _PM.activePower1.power + " with " + selectedPower.power + " in Slot " + _slotNum + "?");
                    break;
                }
            case (2):
                {
                    warnSlot = 2; //< used for overwriting
                    warningText.text = ("Overwrite " + _PM.activePower2.power + " with " + selectedPower.power + " in Slot " + _slotNum + "?");
                    break;
                }
        }        
    }

    public void Overwrite()
    {
        switch (warnSlot)
        {
            case (1):
                {
                    if (_PM.activePower2 == selectedPower)
                    {
                        _PM.activePower2 = null; // remove selected power assignment from other keys
                    }

                    _PM.activePower1 = selectedPower;
                    selectedPower.activeSlot = 1;
                    equippedText.text = "Equipped: Slot 1";
                    break;
                }
            case (2):
                {
                    if (_PM.activePower1 == selectedPower)
                    {
                        _PM.activePower1 = null; // remove selected power assignment from other keys
                    }

                    _PM.activePower2 = selectedPower;
                    selectedPower.activeSlot = 2;
                    equippedText.text = "Equipped: Slot 2";
                    break;
                }
        }
        warningPanel.SetActive(false);
    }

    public void CancelOverwrite()
    {
        warningPanel.SetActive(false);
    }

    public void ChangeGameState(GameState _gameState)
    {
        switch(_gameState)
        {
            case GameState.PAUSED:
                {
                    inGamePanel.SetActive(false);
                    powerTreePanel.SetActive(false);
                    break;
                }
            case GameState.POWERMENU:
                {
                    inGamePanel.SetActive(false);
                    powerTreePanel.SetActive(true);
                    ppText.text = ("PP: " + _GM.powerPoints.ToString());
                    break;
                }
            case GameState.INGAME:
                {
                    ClearUI();
                    inGamePanel.SetActive(true);
                    powerTreePanel.SetActive(false);
                    powerTextSide.SetActive(false);
                    break;
                }
        }
    }


}
