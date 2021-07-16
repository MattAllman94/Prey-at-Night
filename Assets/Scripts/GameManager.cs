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

    public void IncreaseCorruption(float _amount)
    {
        if (currentCorruption < maxCorruption)
        {
            currentCorruption += _amount;

            if (currentCorruption > maxCorruption) // Sets the corruption to the max if 
                currentCorruption = maxCorruption; // it goes over    

            if (currentCorruption > 50)
            {
                corruptionLevel = CorruptionLevel.HIGH;
            }
            else if (currentCorruption > -50)
            {
                corruptionLevel = CorruptionLevel.NORMAL;
            }
            else
            {
                corruptionLevel = CorruptionLevel.LOW;
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
                corruptionLevel = CorruptionLevel.HIGH;
            }
            else if (currentCorruption > -50)
            {
                corruptionLevel = CorruptionLevel.NORMAL;
            }
            else
            {
                corruptionLevel = CorruptionLevel.LOW;
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
        gameState = _gameState;

        switch(gameState)
        {
            case GameState.TITLE:
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    titleCamera.SetActive(true);
                    inGameCamera.SetActive(false);
                    _AM.ChangeBackgroundVolume(0.25f);
                    Cursor.visible = true;
                    break;
                }
            case GameState.INGAME:
                {
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.Locked;
                    titleCamera.SetActive(false);
                    inGameCamera.SetActive(true);
                    _AM.ChangeBackgroundVolume(0.1f);
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

