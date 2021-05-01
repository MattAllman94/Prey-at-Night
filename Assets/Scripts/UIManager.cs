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

    public GameObject unlockButton;
    public TextMeshProUGUI unlockText;

    public GameObject equipPanel;

    public Power selectedPower;

    private void Start()
    {
        powerTreePanel.SetActive(false);
    }

    void Update()
    {
        
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }


    // Pauses Game and reveals skill panel
    void PauseGame()
    {
        gamePaused = !gamePaused;

        if(gamePaused == false)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inGamePanel.SetActive(true);
            powerTreePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inGamePanel.SetActive(false);
            powerTreePanel.SetActive(true);
        }
    }

    
    public void PurchasePower()
    {
        if (_GM.powerPoints >= selectedPower.unlockCost && selectedPower.powerStatus == Power.PowerStatus.Unlocked)
        {
            selectedPower.powerStatus = Power.PowerStatus.Purchased;
            unlockButton.SetActive(false);
            equipPanel.SetActive(true);
        }
    }

    public void EquipPower(string _EquipKey)
    {
        selectedPower.powerStatus = Power.PowerStatus.Active;
        
        switch (_EquipKey)
        {
            case ("1"):
            {
                    if(_PM.activePower2 == selectedPower) // remove selected power assignment from other keys
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
