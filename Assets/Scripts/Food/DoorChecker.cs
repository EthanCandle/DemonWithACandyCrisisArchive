using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChecker : MonoBehaviour
{
    //// this class has a square regualr collider wit ha rigid body, and a sphere trigger child

    //public bool isPlayerWithinRange = false, isPlayerAbleToOpenDoor = false;

    //public string textToShowWhenLocked, textToShoWhenLockedInteract, textToShowWhenOpen;

    //public DialogueManager dm;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    dm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogueManager>();
    //    isPlayerWithinRange = false;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        if (isPlayerWithinRange)
    //        {
    //            if (isPlayerAbleToOpenDoor)
    //            {
    //                TriggerDoorAnimation();

    //            }
    //            else
    //            {
    //                TriggerLockedDoorInteractCanvas();
    //            }

    //        }
    //    }

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    { 
    //        isPlayerWithinRange = true;
    //        dm.TurnOnDialogueWindow();
    //        if (!isPlayerAbleToOpenDoor)
    //        {
    //            TriggerLockedDoorCanvas();
    //        }
    //        else
    //        {
    //            TriggerOpenDoorCanvas();
    //        }
    //        // need to set 2 canvases here, one for not enough candy yet and one for beign able to
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {

    //        isPlayerWithinRange = false;
    //        DisableAllText();
    //    }
    //}

    //public void BecomeOpenable()
    //{
    //    // called by the food manager
    //    isPlayerAbleToOpenDoor = true;
    //}

    //public void TriggerDoorAnimation()
    //{
    //    // play the animation, turn on the timeline objects like the new camera
    //    // disable player shit
    //    // play open sound effect

    //}

    //public void TriggerLockedDoorCanvas()
    //{
    //    // when the player enters the trigger without interacting
    //    dm.ShowDialogueText(textToShowWhenLocked);
    //    // maybe play locked sound effect
    //}
    // public void TriggerLockedDoorInteractCanvas()
    // {
    //    // when the player interacts with the locked door
    //    dm.ShowDialogueText(textToShoWhenLockedInteract);

    //    // maybe play locked sound effect
    //}

    //public void TriggerOpenDoorCanvas()
    //{
    //    // when the player enters teh range
    //    dm.ShowDialogueText(textToShowWhenOpen);
    //    // maybe play locked sound effect
    //}

    //public void DisableAllText()
    //{
    //    // remove text
    //    dm.TurnOffDialogueWindow();

    //}

}
