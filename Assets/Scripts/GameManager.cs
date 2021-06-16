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
        IncreaseCorruption(0);
        
    }

    void Update()
    {
        if (debug) // All Debugging checks
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                DecreaseCorruption(4f);           // FOR TESTING CORRUPTION
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                IncreaseCorruption(4f);           // FOR TESTING CORRUPTION
            }
        }
       

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameState == GameState.INGAME)
                ChangeGameState(GameState.POWERMENU);
            else if (gameState == GameState.POWERMENU)
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

    public void ChangeBlood(float _blood, bool increase = false) // Dont have to put in the bool if increasing blood 
    {
        currentBlood = increase ? currentBlood += _blood : currentBlood -= _blood;
        _UI.UpdateBlood(currentBlood);
    }

    void ChangeGameState(GameState _gameState)
    {
        gameState = _gameState;

        switch(gameState)
        {
            case GameState.TITLE:
                {
                    break;
                }
            case GameState.INGAME:
                {
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.Locked;
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
                    break;
                }
            case GameState.WONGAME:
                {
                    break;
                }
        }

        _UI.ChangeGameState(gameState);
    }
}
