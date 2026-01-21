using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    public Outline outlineToChange; // candy variable of it

    public FoodManager fm;
    public TextMeshProUGUI totalCandyText;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject gmObject = GameObject.FindGameObjectWithTag("GameManager");
        if (gmObject != null)
        {
            fm = gmObject.GetComponent<FoodManager>();
        }
        SetCandyText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCandyText()
    {
        if(fm == null)
        {
            totalCandyText.text = $"NA/NA";
            return;
        }
        totalCandyText.text = $"{fm.candyToCollectTotal}/{fm.candyTotalInScene}";
    }

    public void ToggleCandyText(bool state)
    {
        totalCandyText.gameObject.SetActive(state);
        SetCandyText();
    }

    public void ChangeOutline(bool state)
    {
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
        if (state)
        { 
            foreach (GameObject food in foodObjects)
            {
                Outline outline = food.GetComponent<Outline>();
                if (outline != null)
                {
                    // Example: change outline color or enable it
                    outline.OutlineMode = Outline.Mode.OutlineAll;
                    outline.outlineWidthCurrent = 20;
                }
            }
        }
        else
        {
            foreach (GameObject food in foodObjects)
            {
                Outline outline = food.GetComponent<Outline>();
                if (outline != null)
                {
                    // Example: change outline color or enable it
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                    outline.outlineWidthCurrent = outlineToChange.outlineWidthHolder;
                }
            }
        }
    }

}
