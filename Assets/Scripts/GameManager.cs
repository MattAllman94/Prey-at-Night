using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CorruptionLevel
{
    LOW,
    NORMAL,
    HIGH,
}
   


public class GameManager : Singleton<GameManager>
{
    [Header ("Corruption --------------------------------")]  
    public float currentCorruption = 0f;
    public float maxCorruption = 100f;
    public float minCorruption = -100f;

    public CorruptionLevel corruptionLevel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))  
        {
            DecreaseCorruption(4f);           // FOR TESTING CORRUPTION
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            IncreaseCorruption(4f);           // FOR TESTING CORRUPTION
        }
    }

    public void IncreaseCorruption(float _amount)
    {
        if (currentCorruption < maxCorruption)
        {
            currentCorruption += _amount;

            if (currentCorruption > maxCorruption) // Sets the corruption to the max if 
                currentCorruption = maxCorruption; // it goes over                              
        }
    }

    public void DecreaseCorruption(float _amount)
    {
        if (currentCorruption > minCorruption)
        {
            currentCorruption -= _amount;

            if (currentCorruption < minCorruption) // Sets the corruption to the min if
                currentCorruption = minCorruption; // it goes under      
            


        }
    }
}
