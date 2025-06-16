using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemData : MonoBehaviour
{
    public string nameOfItem;
    public int costOfItem;
    public bool isPurchased, isEnabled;
    public GameObject button;
    public Button buttonAsset;
    public TextMeshProUGUI buttonText;
    private void Awake()
    {
        // buttonAsset = GetComponent<Button>();

        // set price, will getr removed if already purchased

        if (!isPurchased)
        {
            print("Set text of cost");
            buttonText.text = costOfItem.ToString();
        }

    }

    public void HasBeenPurchased()
    {
        isPurchased = true;
    }

    public void ChangeButtonColor(Color colorToChangeTo)
    {

        buttonAsset.image.color = colorToChangeTo;
    }

    public ShopItemDataLowLevel GetSaveData()
    {
        return new ShopItemDataLowLevel
        {
            nameOfItem = nameOfItem,
            isPurchased = isPurchased,
            isEnabled = isEnabled
        };
    }

}

// list of all buttons from where we get the shop item data from
// at start go through all of them and compare their name of item to
// the dictionary of bought items
// if true, then trigger HasBeenPurchased to make it unpurchasable and make the button faded


// If clicked again it goes to the debugStore and turn on/off the item
// it goes through teh dictionary to see which name it is and does the corrosponding function
// 