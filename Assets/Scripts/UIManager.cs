using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class UIManager : Singleton<UIManager>
{
    public GameObject bloodUI;
    public GameObject inGamePanel;
    public GameObject powerTreePanel;

    public TextMeshProUGUI powerName;
    public TextMeshProUGUI powerDesc;

    public bool gamePaused = false;



    private void Start()
    {
        powerTreePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && bloodUI.transform.localScale.y < 1f)  // FOR TESTING BLOOD UI
            AddBlood(0.1f);

        if (Input.GetKeyDown(KeyCode.J) && bloodUI.transform.localScale.y > 0f)  // FOR TESTING BLOOD UI
            ReduceBlood(0.2f);

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

    
    


    // FOR TESTING BLOOD LEVEL FILL EFFECT
    void AddBlood(float _bloodValue)
    {
        bloodUI.transform.localScale += new Vector3(0, _bloodValue, 0);

        if (bloodUI.transform.localScale.y > 1)
            bloodUI.transform.localScale = new Vector3(1, 1, 1);

    }

    void ReduceBlood(float _bloodValue)
    {
        bloodUI.transform.localScale -= new Vector3(0, _bloodValue, 0);

        if (bloodUI.transform.localScale.y < 0)
            bloodUI.transform.localScale = new Vector3(1, 0, 1);
    }
}
