using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CandyNumberShower : MonoBehaviour
{
    public TextMeshProUGUI candyNumberText;
    public int candyAmount = 0;

    public void SetCandyAmount(int candyValue)
    {
        candyAmount = candyValue;
        candyNumberText.text = candyAmount.ToString();
    }


}
