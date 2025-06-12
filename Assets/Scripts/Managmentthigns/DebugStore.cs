using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;


public class DebugStore : MonoBehaviour
{
    // need to hard code each debug aspect in this script.
    // It should independently turn on/off stuff when not in the main menu
    // should be attached to player controller just like the options menu
    // should be free balling in main menu with shouldTriggerEffects = false

    public static DebugStore debugStore;
    public DebugStats debugStatsLocal;

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

        LoadData();
        //DebugStats debugStats = new DebugStats { candyAmount = 0 };

        //string json = JsonUtility.ToJson(debugStats);
        //print(json);

        //DebugStats debugStatsLoaded = JsonUtility.FromJson<DebugStats>(json);
        //print(debugStatsLoaded.candyAmount);
    }

    public void SaveData()
    {
        DebugStats debugStatsToSave;
        if (debugStatsLocal == null)
        {
             debugStatsToSave = new DebugStats { candyAmount = 0 };

        }
        else
        {
             debugStatsToSave = new DebugStats { candyAmount = debugStatsLocal.candyAmount };

        }

        string json = JsonUtility.ToJson(debugStatsToSave);

        File.WriteAllText(Application.dataPath + "/save.txt", json);
        print(json);
    }

    public void LoadData()
    {
        if (!File.Exists(Application.dataPath + "/save.txt"))
        {
            print("Doesn't exist!");
            // if we don't have a save yet, make one
            SaveData();
        }
        string saveString = File.ReadAllText(Application.dataPath + "/save.txt");

        debugStatsLocal = JsonUtility.FromJson<DebugStats>(saveString);
        print(debugStatsLocal.candyAmount);
        TriggerEventListener();
        SaveData();
    }

    public void DeleteData()
    {
        if (!File.Exists(Application.dataPath + "/save.txt"))
        {
            print("Doesn't exist!");
            return;
        }

        // delete
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

    public event Action<int> SetCandyAmount;
    public void OnCandyCollected()
    {
        // this is the function that gets called when the player types the correct word, needs to subscribe to the inputtyper
        // this will call every script that subscribed to this function (UI mainly)

        debugStatsLocal.candyAmount++;
        TriggerEventListener();

        SaveData();
    }

    public void TriggerEventListener()
    {
        if (SetCandyAmount != null)
        {
            // increment by 1 when collecting a candy

            print("IN listener");
            print(debugStatsLocal.candyAmount);
            SetCandyAmount(debugStatsLocal.candyAmount);
        }
    }

    private void OnDestroy()
    {
        SetCandyAmount = null; // removes all listeners
                              // InputAcceptor.current.GotCorrectWord -= CorrectWord; // makes it so the Gamemanager will call the function when the InputAcceptor gets the right word
    }
}
