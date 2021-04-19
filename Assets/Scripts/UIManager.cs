using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject bloodUI;


   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && bloodUI.transform.localScale.y < 1f)  // FOR TESTING BLOOD UI
            AddBlood(0.1f);

        if (Input.GetKeyDown(KeyCode.J) && bloodUI.transform.localScale.y > 0f)  // FOR TESTING BLOOD UI
            ReduceBlood(0.2f);
    }


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
