using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : Prey
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _PROMPT.ChangeState(PromptState.Eleven);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _PROMPT.ChangeState(PromptState.Blank);
        }
    }

}
