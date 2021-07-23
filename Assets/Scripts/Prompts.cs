using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PromptState
{
    Blank, Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine
}

public class Prompts : Singleton<Prompts>
{
    public PromptState myState;

    public float delay = 5f;
    bool messageRecieved = false;
    bool powerPrompt = false;

    public void Start()
    {
        ChangeState(PromptState.Blank);
    }

    public void Update()
    {
        Debug.Log(myState);
    }

    public void ChangeState(PromptState _state)
    {
        myState = _state;

        switch(myState)
        {
            case PromptState.Blank:
                _UI.promptPanel.SetActive(false);
                _UI.prompts.text = "";
                break;
            case PromptState.Zero:
                StartCoroutine(MessageZero());
                break;
            case PromptState.One:
                StartCoroutine(MessageOne());
                break;
            case PromptState.Two:
                StartCoroutine(MessageTwo());
                break;
            case PromptState.Three:
                StartCoroutine(MessageThree());
                break;
            case PromptState.Four:
                StartCoroutine(MessageFour());
                break;
            case PromptState.Five:
                StartCoroutine(MessageFive());
                break;
            case PromptState.Six:
                StartCoroutine(MessageSix());
                break;
            case PromptState.Seven:
                StartCoroutine(MessageSeven());
                break;
            case PromptState.Eight:
                StartCoroutine(MessageEight());
                break;
            case PromptState.Nine:
                StartCoroutine(MessageNine());
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if(other.gameObject.CompareTag("Player") && !messageRecieved)
        {
            //Debug.Log("Player Entered");
            ChangeState(PromptState.Zero);
        }
    }

    #region Messages
    IEnumerator MessageZero()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[0];
        yield return new WaitForSeconds(delay);
        messageRecieved = true;
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.One);
    }
    IEnumerator MessageOne()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[1];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.Two);
    }
    IEnumerator MessageTwo()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[2];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.Three);
    }
    IEnumerator MessageThree()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[3];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageFour()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[4];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        if (!powerPrompt)
        {
            ChangeState(PromptState.Six);
            powerPrompt = true;
        }
        else
        {
            ChangeState(PromptState.Blank);
        }
    }
    IEnumerator MessageFive()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[5];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        if (!powerPrompt)
        {
            ChangeState(PromptState.Six);
            powerPrompt = true;
        }
        else
        {
            ChangeState(PromptState.Blank);
        }
    }
    IEnumerator MessageSix()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[6];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageSeven()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[7];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageEight()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[8];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageNine()
    {
        _UI.promptPanel.SetActive(true);
        _UI.prompts.text = _UI.messages[9];
        yield return new WaitForSeconds(delay);
        _UI.promptPanel.SetActive(false);
        ChangeState(PromptState.Blank);
    }
    #endregion
}
