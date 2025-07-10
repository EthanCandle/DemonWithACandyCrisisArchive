using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

// NotStarted = player hasn't talked to them yet, Locked is player is in the locked dialogue, normal is normal, finished is after talking to them normal
public enum DialogueState {NotStarted, Locked, Normal,Finished  }
public class DialogueInteraction : MonoBehaviour
{
    public DialogueState dialogueState = DialogueState.NotStarted;

    public bool isPlayerWithinRange = false, isPlayerAbleToTalkToMe = false, shouldKillSelfAtEnd = false, isDead;

    public DialogueManager dm;

    public string promptToShow; // door can be (locked)

    public int lineOn = 0;
    public bool hasStartedConversation = false, hasFinishedConversation = false; // started changes, finished is only once
    public bool isTypingOutText = false; // is the text still being written out, false means its been fully typed
    public string animationNamePrevious;

    public DialogueData dialogueDataCurrent; // the current data we are looking at
    public List<DialogueData> dialougeToShowCurrent;
    public List<DialogueData> dialougeToShowLocked; // before player is suppose to talk to them
    public List<DialogueData> dialougeToShowNormal; // the correct dialouge + animation 
    public List<DialogueData> dialougeToShowFinished; // the text to tell the player what to do


    public InputManager _input;
    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogueManager>();

        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {

        // if player presses E,
        // if player is within range
        if (_input.interact && isPlayerWithinRange)
        {
            _input.interact = false;
            _input.talk = false;
            _input.dash = false;
            _input.jumpHold = false;
            _input.jump = false;
            //print("talk");
            Interact();
        }
        else if (hasStartedConversation && _input.talk && isPlayerWithinRange)
        {
            _input.interact = false;
            _input.talk = false;
            _input.dash = false;
            _input.jumpHold = false;
            _input.jump = false;
            Interact();
        }



    }

    public void Interact()
    {

        // then branch if player can't talk to them
        // then branch again if the player has already talked to them
        // whether or noth the player has started it yet
        if (!hasStartedConversation)
        {
            StartConversation();
            return;
        }

        // if the text hasn't fully been typed out
        if (!HasManagerFinishedTypingOutText())
        {
           // print("Finishing the text now");
            // call function that will fully write out the text and prevent the next verse from being written
            DisplayLineImmediatly();
            return;
        }
      //  print("Going onto next line/exiting");
        ContinueConversation();
    }

    public void StartConversation()
    {
        TurnOffPromptWindow(); // turn off the prompt to talk
        TurnOnDialogueWindow(); // turn on the dialogue box
        hasStartedConversation = true;
        dm.TurnOffPlayerMovement(); // turn off player movement

        if(!isPlayerAbleToTalkToMe)
        {
            dialogueState = DialogueState.Locked;
            dialougeToShowCurrent = dialougeToShowLocked;
        }
        else if (hasFinishedConversation)
        {
            dialogueState = DialogueState.Finished;
            dialougeToShowCurrent = dialougeToShowFinished;
        }
        else
        {
            dialogueState = DialogueState.Normal;
            dialougeToShowCurrent = dialougeToShowNormal;
        }
        lineOn = 0;
        DisplayNextLine(); // displays the line
    }


    public void ContinueConversation()
    {
        lineOn += 1;

        // if we are on the last line of dialogue
        if (IsOnLastLineOfDialogue())
        {

            FinishedTalking();

            // if the player is in the npcs normal dialogue route
            if (dialogueState == DialogueState.Normal)
            {
                hasFinishedConversation = true;

                if (shouldKillSelfAtEnd)
                {
                    isDead = true;
                    Destroy(gameObject, 3f);
                }
            }
            return;
        }

         DisplayNextLine(); // displays the line
        // also need to play the function, the animation, and assign the name and sprite
    }

    public bool IsOnLastLineOfDialogue()
    {
        return lineOn > dialougeToShowCurrent.Count - 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isDead)
        {
            isPlayerWithinRange = true;

            TurnOnPromptWindow();
            DisplayPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isDead)
        {
            FinishedTalking();
        }
    }

    // called either by finishing the dialogue or by leaving their range
    public void FinishedTalking()
    {
        if (!hasStartedConversation)
        {
            TurnOffPromptWindow();
        }
        else
        {
            TurnOffDialogueWindow();
            TurnOffPromptWindow();
        }
        isPlayerWithinRange = false;
        dm.TurnOnPlayerMovement();
        hasStartedConversation = false;
    }

    public bool HasManagerFinishedTypingOutText()
    {
        return dm.dialogueText.text == dialogueDataCurrent.dialogueText;
    }

    public void DisplayLineImmediatly()
    {
        dm.FinishTypingTextImmediatly();
    }

    public void DisplayNextLine()
    {
        dialogueDataCurrent = dialougeToShowCurrent[lineOn];
        DisplayText();
        PlayFunction();

        PlayAnimation();
    }

    public void DisplayText()
    {
        dm.ShowDialogueText(dialogueDataCurrent);
    }

    public void PlayFunction()
    {
        // play the function associted with the dialogue, will silently fail if it
        dialogueDataCurrent.functionToCall.Invoke();
    }

    public void PlayAnimation()
    {

        if(animationNamePrevious == dialogueDataCurrent.AnimationToPlayName)
        {
            return;
        }
        // if we need to play the transition animation before goign into the actual animation/pose
        if (dialogueDataCurrent.shouldTransitionToThisAnimation)
        {
            PlayTransitionAnimation();
        }
        else
        {
            // play the normal animation
            PlayNormalAnimation();
        }

    }

    public void PlayNormalAnimation()
    {
        //dialogueDataCurrent.objectAnimator.Play(dialogueDataCurrent.AnimationToPlayName);
        dialogueDataCurrent.objectAnimator.SetTrigger(dialogueDataCurrent.AnimationToPlayName);
        animationNamePrevious = dialogueDataCurrent.AnimationToPlayName;
    }

    public void PlayTransitionAnimation()
    {
        dialogueDataCurrent.objectAnimator.Play(dialogueDataCurrent.dialogueAsset.animationTransitionName);
        StartCoroutine(DelayAnimation());
    }

    public IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(dialogueDataCurrent.dialogueAsset.delayWithinTransition);
        PlayNormalAnimation();
    }

    public void DisplayPrompt()
    {
        if (!isPlayerAbleToTalkToMe)
        {
            dm.ShowPromptText(promptToShow + "(Locked)");
        }
        else
        {
            dm.ShowPromptText(promptToShow);
        }

    }

    public void TurnOnDialogueWindow()
    {
        dm.TurnOnDialogueWindow();
    }

    public void TurnOffDialogueWindow()
    {
        dm.TurnOffDialogueWindow();
    }

    public void TurnOnPromptWindow()
    {
        dm.TurnOnPromptWindow();
    }

    public void TurnOffPromptWindow()
    {
        dm.TurnOffPromptWindow();
    }

    public void TurnOnAbleToTalkTo()
    {
        isPlayerAbleToTalkToMe = true;
    }
}
