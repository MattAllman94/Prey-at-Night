using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PromptState
{
    Blank, Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Eleven
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
        //Debug.Log(myState);
    }

    public void ChangeState(PromptState _state)
    {
        myState = _state;

        

        switch(myState)
        {
            case PromptState.Blank:
                _UI.promptPanel.GetComponent<RectTransform>().DOLocalMoveY(210f, 2f);
                break;
            case PromptState.Zero:
                StartCoroutine(ChangeMessage(0, PromptState.One));
                break;
            case PromptState.One:
                StartCoroutine(ChangeMessage(1, PromptState.Two));
                break;
            case PromptState.Two:
                StartCoroutine(ChangeMessage(2, PromptState.Three));
                break;
            case PromptState.Three:
                StartCoroutine(ChangeMessage(3));
                break;
            case PromptState.Four:
                if (!powerPrompt)
                {
                    StartCoroutine(ChangeMessage(4, PromptState.Six));
                    powerPrompt = true;
                }             
                break;
            case PromptState.Five:
                if (!powerPrompt)
                {
                    StartCoroutine(ChangeMessage(5, PromptState.Six));
                    powerPrompt = true;
                }
                break;
            case PromptState.Six:
                StartCoroutine(ChangeMessage(6));
                break;
            case PromptState.Seven:
                StartCoroutine(ChangeMessage(7));
                break;
            case PromptState.Eight:
                StartCoroutine(ChangeMessage(8));
                break;
            case PromptState.Nine:
                StartCoroutine(ChangeMessage(9));
                break;
            case PromptState.Ten:
                StartCoroutine(ChangeMessage(10));
                break;
            case PromptState.Eleven:
                StartCoroutine(ChangeMessage(11));
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if(other.gameObject.CompareTag("Player") && !messageRecieved)
        {
            //Debug.Log("Player Entered");
            messageRecieved = true;
            ChangeState(PromptState.Zero);
        }
    }

    #region Messages

    IEnumerator ChangeMessage(int _msgNum, PromptState _nextState = PromptState.Blank)
    {
        _UI.promptPanel.GetComponent<RectTransform>().DOLocalMoveY(-330f, 1f);
        _UI.prompts.text = _UI.messages[_msgNum];
        yield return new WaitForSeconds(delay);        
        ChangeState(_nextState);
    }

    IEnumerator MessageZero()
    {
        print("msg ZERO");
        _UI.promptPanel.GetComponent<RectTransform>().DOLocalMoveY(-330f, 1f);
        _UI.prompts.text = _UI.messages[0];
        yield return new WaitForSeconds(delay);
        messageRecieved = true;
        ChangeState(PromptState.One);
    }
    IEnumerator MessageOne()
    {
        _UI.prompts.text = _UI.messages[1];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Two);
    }
    IEnumerator MessageTwo()
    {
        _UI.prompts.text = _UI.messages[2];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Three);
    }
    IEnumerator MessageThree()
    {
        _UI.prompts.text = _UI.messages[3];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Blank);

    }
    IEnumerator MessageFour()
    {
        _UI.prompts.text = _UI.messages[4];
        yield return new WaitForSeconds(delay);     
    }
    IEnumerator MessageFive()
    {
        _UI.prompts.text = _UI.messages[5];
        yield return new WaitForSeconds(delay);        
    }
    IEnumerator MessageSix()
    {
        _UI.prompts.text = _UI.messages[6];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageSeven()
    {
        _UI.prompts.text = _UI.messages[7];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageEight()
    {
        _UI.prompts.text = _UI.messages[8];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageNine()
    {
        _UI.prompts.text = _UI.messages[9];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageTen()
    {
        _UI.prompts.text = _UI.messages[10];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Blank);
    }
    IEnumerator MessageEleven()
    {
        _UI.prompts.text = _UI.messages[11];
        yield return new WaitForSeconds(delay);
        ChangeState(PromptState.Blank);
    }
    #endregion
}
