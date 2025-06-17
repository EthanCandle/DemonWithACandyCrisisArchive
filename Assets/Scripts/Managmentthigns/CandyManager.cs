using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    public Outline outlineToChange; // candy variable of it
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
