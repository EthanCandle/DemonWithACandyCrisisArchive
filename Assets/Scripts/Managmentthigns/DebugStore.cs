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
    public ShopItemCollection shopItemCollectionLocal;

    public bool isInDebugStore = false, shouldTriggerEffects = false;
    public AudioManager audioManager;
    public int volume;

    public bool isMutedSFX = false, isMutedMusic = false;
    public GameObject muteObjectSFX, unMuteObjectSFX, muteObjectMusic, unMuteObjectMusic;
    public Animator debugStoreAnimator;
    public CanvasGroup menuToDeactiveOnSummon; // should be main menu or pause menu

    public List<GameObject> buttonObjects;

    public static readonly Color NormalColor = Color.black;                     // Black
    public static readonly Color CantAffordColor = new Color(0.3f, 0.3f, 0.3f); // Dark Grey
    public static readonly Color CanAffordColor = Color.white;                 // White
    public static readonly Color EnabledColor = Color.green;                   // Green
    public static readonly Color DisabledColor = Color.red;

    public Dictionary<string, Action<bool>> functionDictionary;

    public string saveLocation = Application.dataPath + "/save.txt";
    public string shopItemLocation = Application.dataPath + "/shopItem.txt";
    public string skinLocation = Application.dataPath + "/skin.txt";


    public PlayerManager playerManager;
    public CandyManager candyManager;
    public MainMenu mainMenuScript;
    // Start is called before the first frame update
    void Start()
    {
        PopulateDictionary();
        SetVariables();
        FunctionIfNotInMainMenu();
        saveLocation = Application.dataPath + "/save.txt";
        shopItemLocation = Application.dataPath + "/shopItem.txt";
        LoadCandyData();

        LoadButtonData();
        TriggerAllItems();
        //DebugStats debugStats = new DebugStats { candyAmount = 0 };

        //string json = JsonUtility.ToJson(debugStats);
        //print(json);

        //DebugStats debugStatsLoaded = JsonUtility.FromJson<DebugStats>(json);
        //print(debugStatsLoaded.candyAmount);
    }
    public void PopulateDictionary()
    {
        functionDictionary = new Dictionary<string, Action<bool>>()
        {
            {"trail_runner", TrailRunner},
            {"speed_boost", IncreasePlayerSpeed},
            {"speed_penalty", DecreasePlayerSpeed},
            {"dash_distance_increase", IncreasePlayerDashDistance},
            {"dash_distance_decrease", DecreasePlayerDashDistance},
            {"candy_outline", ChangeCandyOutline},
            {"skin_random", ChangePlayerSkin},
        };
    }
    public void SetVariables()
    {
        debugStore = this;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();


    }

    public void FunctionIfNotInMainMenu()
    {
        if (!shouldTriggerEffects)
        {
            return;
        }
        // shouldn't trigger if in main menu
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

    }
    public void SaveCandyData()
    {
        DebugStats debugStatsToSave;
        if (debugStatsLocal == null)
        {
             debugStatsToSave = new DebugStats { candyAmount = 0, skinNumber = 0};

        }
        else
        {
             debugStatsToSave = new DebugStats { candyAmount = debugStatsLocal.candyAmount, skinNumber = playerManager.skinNumber};

        }

        string json = JsonUtility.ToJson(debugStatsToSave);

        File.WriteAllText(saveLocation, json);
 
    }

    public void LoadCandyData()
    {
        if (!File.Exists(saveLocation))
        {
            // if we don't have a save yet, make one
            SaveCandyData();
        }
        string saveString = File.ReadAllText(saveLocation);

        debugStatsLocal = JsonUtility.FromJson<DebugStats>(saveString);
       
        TriggerEventListener();
    }

    public void DeleteData()
    {
        if (mainMenuScript)
        {
            mainMenuScript.SpawnConfirmationPopup(DeleteDataLogic);
        }
        else
        {
            print("No main menu to spawn are you sure, still deleteing data");
            DeleteDataLogic();
        }

        // delete
    }
    
    public void DeleteDataLogic()
    {
        // deletes all the files save, but then adds them back with the default settings
        print("deleted everything");
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);

        }
        if (File.Exists(shopItemLocation))
        {
            File.Delete(shopItemLocation);
        }
        LoadCandyData(); // saves and loads
        TriggerAllItems(); // makes everything remove itself and it does it

    }
    public void ToggleOptionsMenu()
    {
        if (isInDebugStore)
        {
            DesummonDebugMenu();
        }
        else
        {
            SummonDebugMenu();
        }
    }

    // called by buttons, 
    public void SummonDebugMenu()
    {
        if (!isInDebugStore)
        {
            debugStoreAnimator.SetTrigger("Move");
        }
        // called by options button
        isInDebugStore = true;
        debugStoreAnimator.SetTrigger("Move");
        menuToDeactiveOnSummon.interactable = false;
        LoadButtonData();
    }

    public void DesummonDebugMenu()
    {
        if (isInDebugStore)
        {
            debugStoreAnimator.SetTrigger("Move");
        }
        // called by back button
        isInDebugStore = false;

        menuToDeactiveOnSummon.interactable = true;
       // TriggerAllItems(); 
       // each button will trigger itself so we don't need to call it other than at the start
    }

    public event Action<int> SetCandyAmount;
    public void OnCandyCollected()
    {
        // this is the function that gets called when the player types the correct word, needs to subscribe to the inputtyper
        // this will call every script that subscribed to this function (UI mainly)
        IncreaseCandyAmount(1); // could add a multipler for more candy per thing

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



    public void IncreaseCandyAmount(int amountToChange)
    {
        ChangeCandyAmount(amountToChange);
    }

    public void DecreaseCandyAmount(int amountToChange)
    {
        ChangeCandyAmount(-amountToChange);
    }

    public void ChangeCandyAmount(int amountToChange)
    {
        debugStatsLocal.candyAmount += amountToChange;

        TriggerEventListener();

        SaveCandyData();
    }

    public void SaveAllButtons()
    {
        shopItemCollectionLocal = new ShopItemCollection();
        for (int i = 0; i < buttonObjects.Count; i++)
        {
            shopItemCollectionLocal.items.Add(buttonObjects[i].GetComponent<ShopItemData>().GetSaveData());
        }
        string json = JsonUtility.ToJson(shopItemCollectionLocal, true);


        File.WriteAllText(shopItemLocation, json);
    }


    public void LoadButtonData()
    {

   
        if (!File.Exists(shopItemLocation))
        {

            // if we don't have a save yet, make one
            SaveAllButtons();
        }
        else
        {

        }
        string saveString = File.ReadAllText(shopItemLocation);

        shopItemCollectionLocal = JsonUtility.FromJson<ShopItemCollection>(saveString);

        LoadAllButtons();
    }
    public void LoadAllButtons()
    {

        // trigger on debugStore summon and on every purchase, but not the enable/disable
        for(int i = 0; i < buttonObjects.Count; i++)
        {
            ShopItemData itemData = buttonObjects[i].GetComponent<ShopItemData>();


            for (int j = 0; j < shopItemCollectionLocal.items.Count; j++)
            {
                ShopItemDataLowLevel itemDataSaveDataLowLevel = shopItemCollectionLocal.items[j];
                if (itemDataSaveDataLowLevel.nameOfItem == itemData.nameOfItem)
                {
                    itemData.isEnabled = itemDataSaveDataLowLevel.isEnabled;
                    itemData.isPurchased = itemDataSaveDataLowLevel.isPurchased;

                    break;
                }
            }
            // change button color for feedback
            ChangeButtonColor(itemData);
        }
    }

    public void ChangeButtonColor(ShopItemData itemData)
    {
        // purchased -> enabled = green, disabled = red
        // not purchased -> can afford = white, can't afford = grey
        //        
       // print($"{itemData.nameOfItem}, {itemData.isPurchased},{itemData.isEnabled}");
        if (itemData.isPurchased)
        {
            if (itemData.isEnabled)
            {
                itemData.ChangeButtonColor(EnabledColor);
                itemData.buttonText.text = "Enabled";
            }
            else
            {
                itemData.ChangeButtonColor(DisabledColor);
                itemData.buttonText.text = "Disabled";
            }
        }
        else
        {
            if (CanAfford(itemData))
            {
                itemData.ChangeButtonColor(CanAffordColor);
            }
            else
            {
                itemData.ChangeButtonColor(CantAffordColor);
            }
        }
    }

    public void ToggleEnable(ShopItemData itemData)
    {
        // if player presses button again, it disables it, else it enables it
        if (itemData.isEnabled)
        {
            itemData.isEnabled = false;

        }
        else
        {
            itemData.isEnabled = true;

        }
        InterpreteDictionary(itemData); // when trigger do its thing immedeeitly
    }

    public void EnableTrue(ShopItemData itemData)
    {
        itemData.isEnabled = true;
        ChangeButtonColor(itemData);
    }
    public void EnableFalse(ShopItemData itemData)
    {
        itemData.isEnabled = false;
        ChangeButtonColor(itemData);
    }


    public bool CanAfford(ShopItemData itemData)
    {
        // returns true if cost of item is less than the player's current candy amount
        if (debugStatsLocal.candyAmount >= itemData.costOfItem)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PurchaseSomething(ShopItemData itemData)
    {
        // called by the shop button

        // needs to take in a int value
        // needs to decrement the candy total (update values and save it)
        // needs to pass a string value to go into a dictionary to set it to true
        // needs 2 dictionaries to see if its enabled or disabled
        // needs to disable being able to purchase the item again, should grey it out or x
        if (itemData.isPurchased)
        {
            //print("Already purchased");

            // if the button it clicked then we change enable/disable
            ToggleEnable(itemData);
        }
        else if(debugStatsLocal.candyAmount >= itemData.costOfItem){
            // deduct cost from candy int
            DecreaseCandyAmount(itemData.costOfItem);

            // set item as purchased on itself
            itemData.HasBeenPurchased();
            ToggleEnable(itemData);
        }
        else
        {
            print("Cant afford it");
        }
        ChangeButtonColor(itemData);
        SaveAllButtons();
        LoadAllButtons();
    }



    public void TriggerAllItems()
    {
        //// if we aren't supposed to trigger the effects (only not in main menu) then don't trigger
        //if (!shouldTriggerEffects)
        //{
        //    ChangePlayerSkin()
        //    print("failewd to trigger effects");
        //    return;
        //}
        print("triggered all effects");
        string saveString = File.ReadAllText(shopItemLocation);

        shopItemCollectionLocal = JsonUtility.FromJson<ShopItemCollection>(saveString);

        // go though each item saved and then trigger interpret dictionary
        for (int j = 0; j < shopItemCollectionLocal.items.Count; j++)
        {
            ShopItemDataLowLevel itemDataSaveDataLowLevel = shopItemCollectionLocal.items[j];
            if (!itemDataSaveDataLowLevel.isPurchased)
            {
                continue;
            }
            InterpreteDictionary(itemDataSaveDataLowLevel);
        }
    }
    public void InterpreteDictionary(ShopItemDataLowLevel itemData)
    {
        // go through each thing in the save and do its corrosponding 
        if (functionDictionary.TryGetValue(itemData.nameOfItem, out var action))
        {
            action.Invoke(itemData.isEnabled);
        }
        else
        {
            print($"\"{itemData.nameOfItem}\" function doesn't exist");
        }
    }

    public void InterpreteDictionary(ShopItemData itemData)
    {
        // go through each thing in the save and do its corrosponding 
        if (functionDictionary.TryGetValue(itemData.nameOfItem, out var action))
        {
            action.Invoke(itemData.isEnabled);
        }
        else
        {
            print($"\"{itemData.nameOfItem}\" function doesn't exist");
        }
    }


    public ShopItemData FindItemData(string itemName)
    {
        for (int i = 0; i < buttonObjects.Count; i++)
        {
            ShopItemData itemData = buttonObjects[i].GetComponent<ShopItemData>();
            if(itemData.nameOfItem == itemName)
            {
                return itemData;
            }
        }


        print($"Didn't find Item [{itemName}] DATA ERROR");
        return buttonObjects[0].GetComponent<ShopItemData>();
    }

    public bool CheckIfItemIsEnabled(string itemName)
    {
        ShopItemData itemData = FindItemData(itemName);

        if (itemData.isEnabled)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisableOtherItem(string itemName, bool shouldTrigger = false)
    {
        ShopItemData itemData = FindItemData(itemName);


        if(shouldTrigger)
        {
            // go through each thing in the save and do its corrosponding 
            if (functionDictionary.TryGetValue(itemName, out var action))
            {
                action.Invoke(false);
            }
            else
            {
                print($"\"{itemName}\" function doesn't exist");
            }
        }

        // this applies false in button and save data
        EnableFalse(itemData); 
    

    }

    public void TrailRunner(bool state)
    {
        if (!shouldTriggerEffects)
        {
            return;
        }
        // enable if purchased and enabled, else disable
        playerManager.EnableTrailRunner(state);
    }

    public void IncreasePlayerSpeed(bool state)
    {
        if (state)
        {
            DisableOtherItem("speed_penalty");
        }

        if (!shouldTriggerEffects)
        {
            return;
        }
        print("Increase speed");

        if (!CheckIfItemIsEnabled("speed_penalty"))
        {
            playerManager.IncreasePlayerSpeed(state);
        }
        else
        {
            print("Failed check");
        }
    }

    public void DecreasePlayerSpeed(bool state)
    {
        // if true then disable speed boost
        if (state)
        {
            DisableOtherItem("speed_boost");
        }

        // if we aren't supposed to trigger effects then stop
        if (!shouldTriggerEffects)
        {
            return;
        }
        print(CheckIfItemIsEnabled("speed_boost"));

        // if speed is enabled then don't continue
        if (!CheckIfItemIsEnabled("speed_boost"))
        {
            playerManager.DecreasePlayerSpeed(state);
        }
        else
        {
            print("Failed check");
        }

    }

    public void IncreasePlayerDashDistance(bool state)
    {
        if (state)
        {
            DisableOtherItem("dash_distance_decrease");
        }

        if (!shouldTriggerEffects)
            return;

        if (!CheckIfItemIsEnabled("dash_distance_decrease"))
        {
            playerManager.IncreaseDashDistance(state);
        }
    }

    public void DecreasePlayerDashDistance(bool state)
    {
        // if true then disable speed boost
        if (state)
        {
            DisableOtherItem("dash_distance_increase");
        }

        // if we aren't supposed to trigger effects then stop
        if (!shouldTriggerEffects)
        {
            return;
        }

        // if speed is enabled then don't continue
        if (!CheckIfItemIsEnabled("dash_distance_increase"))
        {
            playerManager.DecreaseDashDistance(state);
        }
    }

    public void ChangeCandyOutline(bool state)
    {        
        // if we aren't supposed to trigger effects then stop
        candyManager.ChangeOutline(state);

    }

    public void ChangePlayerSkin(bool state)
    {
        if (state)
        {
            // if on default then change to something else
            if (debugStatsLocal.skinNumber == 0)
            {
                debugStatsLocal.skinNumber = UnityEngine.Random.Range(1, playerManager.skinObjects.Count);
           
            }

            playerManager.EnableSkin(debugStatsLocal.skinNumber);
        }
        else
        {
            debugStatsLocal.skinNumber += 1;

            if(debugStatsLocal.skinNumber >= playerManager.skinObjects.Count)
            {

                debugStatsLocal.skinNumber = 1;
            }
            // set to default skin
            playerManager.EnableSkin(0);


        }

        SaveCandyData(); // saves skin number 

    }

    //public void ChangePlayerSkin(bool state)
    //{
    //    if (state)
    //    {
    //        // if on default then change to something else
    //        if (debugStatsLocal.skinNumber == 0)
    //        {
    //            debugStatsLocal.skinNumber = UnityEngine.Random.Range(1, playerManager.skinObjects.Count);

    //        }

    //        playerManager.EnableSkin(debugStatsLocal.skinNumber);
    //    }
    //    else
    //    {
    //        int oldNum = debugStatsLocal.skinNumber;
    //        debugStatsLocal.skinNumber = UnityEngine.Random.Range(1, playerManager.skinObjects.Count);
    //        print($"{oldNum}, {debugStatsLocal.skinNumber}");
    //        if (oldNum == debugStatsLocal.skinNumber)
    //        {

    //            // go down a skin if we rolled the same one (alows the OG demon skin
    //            debugStatsLocal.skinNumber--;
    //            if (debugStatsLocal.skinNumber == 0)
    //            {
    //                debugStatsLocal.skinNumber = 2;
    //            }
    //        }
    //        // set to default skin
    //        playerManager.EnableSkin(0);


    //    }

    //    SaveCandyData(); // saves skin number 

    //}

    // other ideas for debug stuff:
    /*
     Bigger outline for candy
    Bigger outline for player
    Change transition time?
    Increase/Decrease dash distance
    Bounce pad going higher?
    Jump higher


    */
}
