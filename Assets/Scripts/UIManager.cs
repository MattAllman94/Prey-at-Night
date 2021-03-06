using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Header("In Game")]
    public GameObject inGamePanel;
    public TextMeshProUGUI cBloodText;
    //public TextMeshProUGUI cHealthText;
    public TextMeshProUGUI prompts;
    public GameObject promptPanel;
    //public Slider healthSlider;
    public Image healthImg;
    public Image bloodImg;

    public Image power1Icon;
    public Image power2Icon;

    [Header("Testing")]
    public TextMeshProUGUI monstersDefeated;
    public GameObject controlPanel;


    [Header("End State")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Paused")]
    public GameObject pausedPanel;

    [Header("Title")]
    public GameObject titlePanel;
    public CanvasGroup fadePanelGroup;
    public GameObject optionsPanel;
    public float cvFadeTime = 2f;

    [Header ("Power Tree")]
    public GameObject powerTreePanel;
    public GameObject powerTextSide;
    public GameObject warningPanel;

    public TextMeshProUGUI powerName;
    public TextMeshProUGUI equippedText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI powerDesc;
    public TextMeshProUGUI ppText;

    public GameObject equipSlot1;
    public Image slot1Icon;
    public GameObject equipSlot2;
    public Image slot2Icon;


    public GameObject unlockButton;
    public TextMeshProUGUI unlockText;

    public GameObject equipPanel;

    public Power selectedPower;

    int warnSlot = 0;

    //public Camera uiCam;

    public string[] messages = new string[]
    {
        "There is a monster up ahead!! \n I need to get stronger if I am to fight this", //0
        "Maybe I should hunt some of the inhabitants of this town?", //1
        "Press 'Left Mouse' to attack", //2
        "Press 'Q' to drain blood from humans", //3
        "I feel a lot stronger already, but I feel my humanity slipping away", //4
        "I feel slightly stronger, but I am containing my humanity", //5
        "Press 'Tab' to open the power menu", //6
        "Press '1' to use your powers", //7
        "There was a rumble when I killed the gargoyle. \n I must be doing something right!", //8
        "What was that roar? It came from the Cathedral! \n I should check it out", //9
        "I feel powerful... But at what cost?", //10
        "There is nothing this way for me, I should turn back" //11

    };

    private void Start()
    {
        //uiCam.orthographicSize = 51;
        //uiCam.orthographicSize = 50;
        powerTreePanel.SetActive(false);
        powerTextSide.SetActive(false);
        UpdateHealth(_P.currentHealth);
    }



    public void Update()
    {

        if(Input.GetKey(KeyCode.P))
        {
            controlPanel.SetActive(true);
        }
        else
        {
            controlPanel.SetActive(false);
        }
    }

    #region Button Functions
    public void PlayGame()
    {
        _GM.LoadData();
        _GM.ChangeGameState(GameState.INGAME);       
    }

    public void Resume()
    {
        _GM.ChangeGameState(GameState.INGAME);
    }

    public void SaveGame()
    {
        _GM.SaveData();
        //Debug.Log("Saved Game");
    }

    public void GoToTitle()
    {
        _GM.ChangeGameState(GameState.TITLE);
    }

    public void Return()
    {
        if (_GM.gameState == GameState.TITLE)
        {
            optionsPanel.SetActive(false);
            titlePanel.SetActive(true);
        }
        else if (_GM.gameState == GameState.PAUSED)
        {
            optionsPanel.SetActive(false);
            pausedPanel.SetActive(true);
        }
    }

    public void Load()
    {
        _GM.LoadData();
        _GM.ChangeGameState(GameState.INGAME);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region POWER TREE
    public void PurchasePower()
    {
        if (_GM.powerPoints >= selectedPower.unlockCost && selectedPower.powerStatus == PowerStatus.Unlocked)
        {           
            _GM.ChangePowerPoints(selectedPower.unlockCost);
            UpdatePowerPoints(_GM.powerPoints);
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
                       // if(_PM.activePower1 == null || _PM.activePower1.power == null)
                        //{
                        //    _PM.activePower1.power = Powers.NoPower;
                        //}
                
                            if (_PM.activePower1 != selectedPower && _PM.activePower1.power != Powers.NoPower) // checks if theres a power on slot 1
                            {
                                //warn Player
                                DisplayWarning(1);
                            }
                            else
                            {
                                if (_PM.activePower2.power == selectedPower.power)
                                {
                                    ClearSlot2();
                                }

                                UpdateSlot1();
                            }
                          
                        
                        break;
                    }
                            
                case ("2"):
                    {
                        if (_PM.activePower2 != null)
                        {
                            if (_PM.activePower2 != selectedPower && _PM.activePower2.power != Powers.NoPower) // checks if theres a power on slot 2
                            {
                                //warn Player
                                DisplayWarning(2);
                            }
                            else
                            {
                                if (_PM.activePower1.power == selectedPower.power)
                                {
                                    ClearSlot1();
                                }

                                UpdateSlot2();
                            }
                        }
                        break;                     
                    }
            }  
        }
        
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
                        ClearSlot2();                 
                    }

                    UpdateSlot1();
                    break;
                }
            case (2):
                {
                    if (_PM.activePower1 == selectedPower)
                    {
                        ClearSlot1();
                    }

                    UpdateSlot2();
                    break;
                }
        }
        warningPanel.SetActive(false);
    }

    void UpdateSlot1()
    {
        _PM.activePower1 = selectedPower;
        selectedPower.activeSlot = 1;
        slot1Icon.sprite = selectedPower.icon;
        power1Icon.sprite = selectedPower.icon;
        equippedText.text = "Equipped: Slot 1";
    }

    void UpdateSlot2()
    {

        _PM.activePower2 = selectedPower;
        selectedPower.activeSlot = 2;
        slot2Icon.sprite = selectedPower.icon;
        power2Icon.sprite = selectedPower.icon;
        equippedText.text = "Equipped: Slot 2";
    }

    void ClearSlot1()
    {
        _PM.activePower1.activeSlot = 0;
        _PM.activePower1.power = Powers.NoPower; // remove selected power assignment from other keys 
        slot1Icon.sprite = null;
        power1Icon.sprite = null;
    }

    void ClearSlot2()
    {   
        _PM.activePower2.activeSlot = 0;
        _PM.activePower2.power = Powers.NoPower; // remove selected power assignment from other keys  
        slot2Icon.sprite = null;
        power2Icon.sprite = null;
    }

    public void CancelOverwrite()
    {
        warningPanel.SetActive(false);
    }

    public void UpdateRightPanel(bool _unlockButton, string _unlockText, string _equipText)
    {
        unlockButton.SetActive(_unlockButton);
        unlockText.text = (_unlockText);
        equippedText.text = _equipText;
    }

    #endregion

    public void UpdateBlood(float _blood)
    {
        cBloodText.text = _blood.ToString("f0");
        bloodImg.fillAmount = MapTo01(_blood, 0f, _GM.maxBlood);
    }

    public void UpdateHealth(float _health)
    {      
        healthImg.fillAmount = MapTo01(_health, 0f, _P.maxHealth);
    }

    public void UpdatePowerPoints(int _points)
    {
        ppText.text = ("PP: " + _points.ToString());
    }

    public void UpdateMonstersDefeated(int _number)
    {
        monstersDefeated.text = _number.ToString() + " / 5";
    }

    public void ClearUI()
    {
        powerName.text = "";
        powerDesc.text = "";
        equippedText.text = "";
        unlockButton.SetActive(false);
        //selectedPower.power = Powers.NoPower;
    }

    IEnumerator StartSequence()
    {
        print("starting sequence");
        StartCoroutine(FadePanelIn(fadePanelGroup));
        yield return new WaitUntil(()=> fadePanelGroup.alpha >= 1f);
        print("entering INGAME");
        _GM.ChangeGameState(GameState.INGAME);
        StartCoroutine(FadePanelOut(fadePanelGroup));
        yield return null;
    }

    public IEnumerator FadePanelIn(CanvasGroup _cg)
    {
        //_cg.DOFade(1, cvFadeTime);

        while (_cg.alpha < 1)
        {
            _cg.alpha += 0.1f;
            yield return null;
        }
        
    }

    public IEnumerator FadePanelOut(CanvasGroup _cg)
    {
        //_cg.DOFade(0f, cvFadeTime);

        while (_cg.alpha > 0)
        {
            _cg.alpha -= 0.1f;
            yield return null;
        }
        
    }

    public void ShowPanel(GameObject _panel)
    {
        inGamePanel.SetActive(false);
        powerTreePanel.SetActive(false);
        pausedPanel.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        titlePanel.SetActive(false);
        optionsPanel.SetActive(false);

        _panel.SetActive(true);
    }

    public void ResetActivePowers()
    {   
        Power a = new Power();
        Power b = new Power();

        a.power = Powers.NoPower;
        b.power = Powers.NoPower;

        _PM.activePower1 = a;
        _PM.activePower2 = b;
    }

    public void ChangeGameState(GameState _gameState)
    {
        switch(_gameState)
        {
            case GameState.PAUSED:
                {
                    ShowPanel(pausedPanel);
                    break;
                }
            case GameState.POWERMENU:
                {
                    //ResetActivePowers();
                    ShowPanel(powerTreePanel);
                    ClearUI();
                    ppText.text = ("PP: " + _GM.powerPoints.ToString());
                    break;
                }
            case GameState.INGAME:
                {
                    ClearUI();
                    ShowPanel(inGamePanel);
                    powerTextSide.SetActive(false);                  
                    break;
                }
            case GameState.TITLE:
                {
                    ShowPanel(titlePanel);
                    powerTextSide.SetActive(false);                
                    break;
                }
        }
    }
}
