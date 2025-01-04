using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FoodManager : MonoBehaviour
{
    // keep track of how much food has been gotten, trigger the door to open to be intereacted with, hold objects needed 
    // for platforming relavence
    // maybe keep player health or bonus or something
    public int candyToCollectNeeded = 10, candyToCollectTotal = 0;

    public GameObject objectToBecomeInteractable; // door to the end the level with

    public GameManager gm; 

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectNormalCandy()
    {
        candyToCollectTotal += 1;
        UpdateUI();
        if (candyToCollectTotal >= candyToCollectNeeded)
        {
            FinishCollecting();
        }

    }

    // probally wont need if going for a shop/collectable, but maybe for a trigger for teh door to be playable
    public void FinishCollecting()
    {
        print("Finished collecting food, door is now interactable");
        // doorObject.GetComponent<DialogueInteraction>().TurnOnAbleToTalkTo(); // makes the girl able to be opened
        objectToBecomeInteractable.GetComponent<DialogueInteraction>().TurnOnAbleToTalkTo();
    }

    // fix
    public void UpdateUI()
    {
        // need some text on the screen to show how much candy is needed

    }
}
