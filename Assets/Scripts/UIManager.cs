using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class UIManager : Singleton<UIManager>
{
    public GameObject inGamePanel;

    public bool gamePaused = false;

    [Header ("Power Tree")]
    public GameObject powerTreePanel;
    public GameObject powerTextSide;

    public TextMeshProUGUI powerName;
    public TextMeshProUGUI powerDesc;
    public TextMeshProUGUI ppText;

    public GameObject unlockButton;
    public TextMeshProUGUI unlockText;

    public GameObject equipPanel;

    public Power selectedPower;

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
            //<
        }
    }

    public void EquipPower(string _EquipKey)
    {
        if(selectedPower.powerStatus == PowerStatus.Purchased)
        {
            selectedPower.powerStatus = PowerStatus.Active;

            switch (_EquipKey)
            {
                case ("1"):
                    {
                        if (_PM.activePower2 == selectedPower) // remove selected power assignment from other keys
                        {
                            _PM.activePower2 = null;
                        }

                        _PM.activePower1 = selectedPower;
                        break;
                    }

                case ("2"):
                    {
                        if (_PM.activePower1 == selectedPower) // remove selected power assignment from other keys
                        {
                            _PM.activePower1 = null;
                        }

                        _PM.activePower2 = selectedPower;
                        break;
                    }
            }  
        }
    }

    public void ClearUI()
    {
        powerName.text = "";
        powerDesc.text = "";
        unlockButton.SetActive(false);
        selectedPower = null;
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
