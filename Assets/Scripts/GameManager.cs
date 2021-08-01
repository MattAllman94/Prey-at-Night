using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    TITLE,
    INGAME,
    PAUSED,
    POWERMENU,
    GAMEOVER,
    WONGAME  
}
    

public enum CorruptionLevel
{
    LOW,
    NORMAL,
    HIGH,
}
   


public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
    public Settings settings;

    public GameObject titleCamera;
    public GameObject inGameCamera;

    [Header ("Corruption ")]  
    public float currentCorruption = 0f;
    public float maxCorruption = 100f;
    public float minCorruption = -100f;

    public CorruptionLevel corruptionLevel;

    [Header("Blood ")]
    public float currentBlood = 0;
    public float maxBlood = 100;

    [Header("Power Points ")]
    public int powerPoints = 0;

    public bool debug = false;



    private void Start()
    {
        LoadData();
        IncreaseCorruption(0);
        ChangeBlood(0);
        ChangeGameState(GameState.TITLE);

    }
    
    public void SaveData()
    {
        _DATA.SetSettings(settings);
        _DATA.SetGameData(currentBlood, currentCorruption, powerPoints);
        _DATA.SetPlayerData();
    }

    public void LoadData()
    {
        settings = _DATA.GetSettings();
        currentBlood = _DATA.GetBloodLevel();
        currentCorruption = _DATA.GetCorruptionLevel();
        powerPoints = _DATA.GetPowerPoints();
        _P.currentHealth = _DATA.GetCurrentHealth();
        _P.transform.position = _DATA.GetLastPosition();
        _P.transform.rotation = _DATA.GetLastRotation();
    }

    void Update()
    {
        if (debug) // All Debugging checks
        {
            //if(Input.GetKeyDown(KeyCode.L))
            //{
            //    SaveData();        // FOR TESTING SAVING
            //    Debug.Log("Saved State");
            //}

            //if (Input.GetKeyDown(KeyCode.J))
            //{
            //    ChangePowerPoints(5, true);        // Testing Power Points
            //}

            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    ChangeBlood(25f, true);        // Testing Power Points
            //}

            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    _NPC.currentMonsters += 1;
            //}

            if(Input.GetKeyDown(KeyCode.Alpha0)) //Testing Corruption levels
            {
                IncreaseCorruption(10);
            }
            if(Input.GetKeyDown(KeyCode.Alpha9))
            {
                DecreaseCorruption(10);
            }
            
            //Debug.Log(currentCorruption + " - " + corruptionLevel);

        }
       

        if (Input.GetKeyDown(KeyCode.Escape) && gameState != GameState.TITLE)
        {
            if (gameState == GameState.INGAME || gameState == GameState.POWERMENU)
                ChangeGameState(GameState.PAUSED);                                      // Pause Game
            else 
                ChangeGameState(GameState.INGAME);
        }

        if(Input.GetKeyDown(KeyCode.Tab) && gameState != GameState.TITLE)
        {
            if (gameState == GameState.INGAME)
                ChangeGameState(GameState.POWERMENU);                                       // Open Power Menu 
            else
                ChangeGameState(GameState.INGAME);                                    
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeState(CorruptionLevel _corrupt)
    {
        corruptionLevel = _corrupt;

        switch (corruptionLevel)
        {
            case CorruptionLevel.LOW:
                _P.normalModel.SetActive(true);
                _P.corruptModel.SetActive(false);
                _P.atkDamage = 20;
                break;
            case CorruptionLevel.NORMAL:
                _P.normalModel.SetActive(true);
                _P.corruptModel.SetActive(false);
                _P.atkDamage = 20;
                break;
            case CorruptionLevel.HIGH:
                _PROMPT.ChangeState(PromptState.Ten);
                _P.normalModel.SetActive(false);
                _P.corruptModel.SetActive(true);
                _P.atkDamage = 40;
                break;
        }
    }

    public void IncreaseCorruption(float _amount)
    {
        if (currentCorruption < maxCorruption)
        {
            currentCorruption += _amount;

            if (currentCorruption > maxCorruption) // Sets the corruption to the max if 
                currentCorruption = maxCorruption; // it goes over    

            if (currentCorruption > 50)
            {
                ChangeState(CorruptionLevel.HIGH);
            }
            else if (currentCorruption < -50)
            {
                ChangeState(CorruptionLevel.LOW);
            }
            else
            {
                ChangeState(CorruptionLevel.NORMAL);
            }
        }
    }

    public void DecreaseCorruption(float _amount)
    {
        if (currentCorruption > minCorruption)
        {
            currentCorruption -= _amount;

            if (currentCorruption < minCorruption) // Sets the corruption to the min if
                currentCorruption = minCorruption; // it goes under      

            if (currentCorruption > 50)
            {
                ChangeState(CorruptionLevel.HIGH);
            }
            else if (currentCorruption < -50)
            {
                ChangeState(CorruptionLevel.LOW);
            }
            else
            {
                ChangeState(CorruptionLevel.NORMAL);
            }
        }
    }

    public void ChangeBlood(float _blood, bool increase = false) // Dont have to put in the bool if decreasing blood 
    {
        currentBlood = increase ? currentBlood += _blood : currentBlood -= _blood;
        if (currentBlood > maxBlood)
            currentBlood = maxBlood;
        _UI.UpdateBlood(currentBlood);
    }

    public void ChangePowerPoints(int _points, bool increase = false) // Dont have to put in the bool if decreasing pp
    {
        powerPoints = increase ? powerPoints += _points : powerPoints -= _points;
        if (powerPoints < 0)
            powerPoints = 0;
        _UI.UpdatePowerPoints(powerPoints);
    }

    public void ChangeGameState(GameState _gameState)
    {
        if (gameState == GameState.TITLE && _gameState == GameState.INGAME)
            _AM.ChangeMusic();


        gameState = _gameState;

        switch(gameState)
        {
            case GameState.TITLE:
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    titleCamera.SetActive(true);
                    inGameCamera.SetActive(false);
                    _AM.ChangeRainVolume(0.25f);
                    _AM.ChangeMusic(false);
                    Cursor.visible = true;
                    break;
                }
            case GameState.INGAME:
                {
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.Locked;
                    titleCamera.SetActive(false);
                    inGameCamera.SetActive(true);
                    _AM.ChangeRainVolume(0.1f);
                    Cursor.visible = false;
                    break;
                }
            case GameState.PAUSED:
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
            case GameState.POWERMENU:
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
            case GameState.GAMEOVER:
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
            case GameState.WONGAME:
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
        }

        _UI.ChangeGameState(gameState);
    }

   
}

[Serializable]
public class Settings
{
    public bool SFX;
    public bool music;
    public bool vfx;

    public Settings(bool _sounds, bool _music, bool _vfx)
    {
        SFX = _sounds;
        music = _music;
        vfx = _vfx;
    }
}

