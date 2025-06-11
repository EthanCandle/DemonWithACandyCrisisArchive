using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DebugStore : MonoBehaviour
{
    // need to hard code each debug aspect in this script.
    // It should independently turn on/off stuff when not in the main menu
    // should be attached to player controller just like the options menu
    // should be free balling in main menu with shouldTriggerEffects = false

    public static DebugStore debugStore;

    public bool isInDebugStore = false, shouldTriggerEffects = false;
    public AudioManager audioManager;
    public int volume;

    public bool isMutedSFX = false, isMutedMusic = false;
    public GameObject muteObjectSFX, unMuteObjectSFX, muteObjectMusic, unMuteObjectMusic;
    public Animator debugStoreAnimator;
    public CanvasGroup menuToDeactiveOnSummon; // should be main menu or pause menu
    // Start is called before the first frame update
    void Start()
    {
        debugStore = this;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        DebugStats debugStats = new DebugStats { candyAmount = 0 };

        string json = JsonUtility.ToJson(debugStats);
        print(json);

        DebugStats debugStatsLoaded = JsonUtility.FromJson<DebugStats>(json);
        print(debugStatsLoaded.candyAmount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void ToggleOptionsMenu()
    {
        if (isInDebugStore)
        {
            DesummonOptionsMenu();
        }
        else
        {
            SummonOptionsMenu();
        }
    }

    // called by buttons, 
    public void SummonOptionsMenu()
    {
        if (!isInDebugStore)
        {
            debugStoreAnimator.SetTrigger("Move");
        }
        // called by options button
        isInDebugStore = true;
        debugStoreAnimator.SetTrigger("Move");
        menuToDeactiveOnSummon.interactable = false;
    }

    public void DesummonOptionsMenu()
    {
        if (isInDebugStore)
        {
            debugStoreAnimator.SetTrigger("Move");
        }
        // called by back button
        isInDebugStore = false;

        menuToDeactiveOnSummon.interactable = true;
    }

    public event Action<int> OnCorrectWord;
    public void CorrectWord()
    {
        // this is the function that gets called when the player types the correct word, needs to subscribe to the inputtyper
        // this will call every script that subscribed to this function (UI mainly)


        if (OnCorrectWord != null)
        {
            OnCorrectWord(1);
        }
    }

    private void OnDestroy()
    {
        OnCorrectWord = null; // removes all listeners
                              // InputAcceptor.current.GotCorrectWord -= CorrectWord; // makes it so the Gamemanager will call the function when the InputAcceptor gets the right word
    }
}
