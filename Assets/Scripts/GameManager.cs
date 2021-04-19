using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Corruption --------------------------------")]  
    public float currentCorruption = 0f;
    public float maxCorruption = 100f;
    public float minCorruption = -100f;
    public bool highCorruption = false;
    public bool lowCorruption = false;

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

            if (currentCorruption > maxCorruption / 2)
            {
                highCorruption = true;
                lowCorruption = false;
            }
            else
            {
                if (currentCorruption < maxCorruption / 2 && currentCorruption > minCorruption / 2)
                {
                    highCorruption = false;
                    lowCorruption = false;
                }
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

            if (currentCorruption < minCorruption / 2)
            {
                lowCorruption = true;
                highCorruption = false;
            }
            else
            {
                if (currentCorruption < maxCorruption / 2 && currentCorruption > minCorruption / 2)
                {
                    highCorruption = false;
                    lowCorruption = false;
                }
            }          
        }
    }
}
