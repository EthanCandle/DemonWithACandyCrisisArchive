using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CandyNumberShower : MonoBehaviour
{
    public TextMeshProUGUI candyNumberText;
    public int candyAmount = 0;

    public DebugStore debugStore;
    private void Awake()
    {

        debugStore = FindObjectOfType<DebugStore>();
        debugStore.SetCandyAmount += SetCandyAmount;
    }
    public void SetCandyAmount(int candyValue)
    {
        print("In shower candy");
        candyAmount = candyValue;
        candyNumberText.text = candyAmount.ToString();
    }


}
