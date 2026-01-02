using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // text to set for the dialogue
    public TextMeshProUGUI promptText; // what action is should be
    public TextMeshProUGUI nameText; // name of character
    public GameObject canvasToShowText; // canvas to turn on/off when dialogue is needed,
    public GameObject canvasToShowPrompt;


    public RawImage avatarImage;
    public GameObject blimp;
    public GameManager gm;

    public float lettersPerSecond = 0.1f; // let the player change this somehow
    public bool finishTypingOutSentence = false;

    public Animator dialogueAnimation;

    public Sound talkingSFX;
    public Sound windowGoingInSFX, windowGoingOutSFX;

    public PlayerInput playerInput;
    public bool isUsingKeyboard = true;

    public SettingsManager settingsManagerScript;

    public Coroutine typingCoroutine;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        settingsManagerScript = GameObject.FindGameObjectWithTag("PlayerMenu").GetComponent<SettingsManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // needs to be called by the other script
    public void TurnOffDialogueWindow()
    {
        // hide the dialouge, re enable player movement
        // canvasToShowText.SetActive(false);
        settingsManagerScript.allowedToPause = true;
        dialogueAnimation.Play("GoingDown");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(windowGoingOutSFX);
        //   TurnOnPlayerMovement();
    }

    public void TurnOnDialogueWindow()
    {
        // have animation of it popping up instead of insta appearing
        //TurnOffPlayerMovement();
        canvasToShowText.SetActive(true);
        settingsManagerScript.allowedToPause = false;
        dialogueAnimation.Play("GoingUp");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(windowGoingInSFX);
    }

        // needs to be called by the other script
    public void TurnOffPromptWindow()
    {
        // hide the dialouge, re enable player movement
        canvasToShowPrompt.SetActive(false);
     //   TurnOnPlayerMovement();
    }

    public void TurnOnPromptWindow()
    {
        // have animation of it popping up instead of insta appearing
        //TurnOffPlayerMovement();
        canvasToShowPrompt.SetActive(true);
    }

    public void ShowDialogueText(DialogueData data)
    {
        // need to replace with typing it out instead like most visual novels
        // should be ienumerator that calls itself in the big for loop

        // stop previous coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeOutText(data.dialogueText));
        // display name and avatar
        avatarImage.texture = data.dialogueAsset.avatarTexture;
        nameText.text = data.dialogueAsset.nameOfSelf;
    }

    IEnumerator TypeOutText(string sentenceToType)
    {
        dialogueText.text = "";  // Clear previous text
        TurnOffBlimp(); // remove the "hey you can go to the next line blimp"
        foreach (char letter in sentenceToType)
        {
            FindObjectOfType<AudioManager>().PlaySoundInstantiate(talkingSFX);
            if (finishTypingOutSentence)
            {
                FinishedTyping();

                dialogueText.text = sentenceToType;
                yield break;
            }


            dialogueText.text += letter;      // Add letter by letter
            yield return new WaitForSeconds(1 / lettersPerSecond);  // Wait
        }
        FinishedTyping();


    }

    public void FinishedTyping()
    {
        TurnOnBlimp();
        finishTypingOutSentence = false; // this bool is to make the ienumator finish typing out
    }

    public void TurnOnBlimp()
    {
        blimp.SetActive(true);
    }

    public void TurnOffBlimp()
    {
        blimp.SetActive(false);
    }

    public void FinishTypingTextImmediatly()
    {
        finishTypingOutSentence = true;
    }

    public void ShowPromptText(string textToShow = "Interact")
    {
        // need to replace with typing it out instead like most visual novels
        // should be ienumerator that calls itself in the big for loop

       // print(playerInput.currentControlScheme);
        if (playerInput.currentControlScheme == "KeyboardMouse" || playerInput.currentControlScheme == "Keyboard")
        {
            promptText.text = $"(E) {textToShow}"; // prints like "(E) Talk"
        }
        else
        {
            promptText.text = $"(RB/North Button) {textToShow}"; // prints like "(E) Talk"
        }


    }
    public void TurnOffPlayerMovement()
    {
        gm.TurnOffPlayerMovement();
    }

    public void TurnOnPlayerMovement()
    {
        gm.TurnOnPlayerMovement();
    }


}
