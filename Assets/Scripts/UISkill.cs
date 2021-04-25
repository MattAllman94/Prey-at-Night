using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : Prey
{
    // Put this script on each Skill in the Skill Panel

    public string mySkill;
    public string myDescription;


    public void Selected()
    {
        _UI.skillTitle.text = (mySkill);
        _UI.skillDesc.text = (myDescription);       
    }
}
