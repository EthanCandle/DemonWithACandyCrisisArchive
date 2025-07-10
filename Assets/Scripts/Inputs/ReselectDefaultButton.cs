using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.InputSystem;

public class ReselectDefaultButton : MonoBehaviour
{
    public GameObject  currentButton; // Assign your default/fallback button in Inspector

    public List<GameObject> defaultButtons;
    public List<Button> validButtonsStore;
    public Button[] allButtonsStore;

    void Update()
    {
        if ((EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy) && Gamepad.current != null)
        {

         //   print("setting random button");
            // Check if the user is trying to navigate with controller
            Vector2 nav = Gamepad.current.leftStick.ReadValue();
            bool dpadPressed = Gamepad.current.dpad.ReadValue() != Vector2.zero;
            bool submitPressed = Gamepad.current.buttonSouth.wasPressedThisFrame; // A / Cross

            if (nav != Vector2.zero || dpadPressed || submitPressed)
            {

            }
            else
            {
              //  print($"{nav} {dpadPressed} {submitPressed}");
            }
           // print($"{EventSystem.current.currentSelectedGameObject == null} {!EventSystem.current.currentSelectedGameObject.activeInHierarchy} {Gamepad.current != null}");
            SelectRandomButton();
        }
        else
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

                Selectable selectable = selectedObj.GetComponent<Selectable>();
                if (selectable != null && selectable.interactable)
                {

                   // Debug.Log($"{selectedObj.name} is interactable!");
                }
                else
                {
                    SelectRandomButton();
                  //  Debug.Log($"{selectedObj.name} is NOT interactable.");
                }
            }

           // print(EventSystem.current.currentSelectedGameObject.name);
        }
    }

    void Start()
    {
        SelectRandomButton();
    }

    public bool CheckButton(GameObject defaultButton)
    {
        if (defaultButton == null || !defaultButton.activeInHierarchy)
        {
          //  print("Null or not active");
            return false;
        }

        // Check if it has a Selectable component (e.g., Button, Toggle, etc.)
        Selectable selectable = defaultButton.GetComponent<Selectable>();
        if (selectable == null || !selectable.interactable)
        {
           // print("Not null or not interactiable");
            return false;
        }

        // Check parent CanvasGroups for interactable = false
        Transform current = defaultButton.transform;
        while (current != null)
        {
            CanvasGroup cg = current.GetComponent<CanvasGroup>();
            if (cg != null && !cg.interactable)
            {
               // print("Null or parent is not interactable");
                return false;
            }


            current = current.parent;
        }

        // this means this button works
        return true;
    }

    public void SelectRandomButton()
    {
        bool foundButton = false;
        int i = 0;
        for(i = 0; i < defaultButtons.Count; i++)
        {
            if (CheckButton(defaultButtons[i]))
            {
                foundButton = true;
            //    print("found button");
                break;
            }
        }

        if (foundButton)
        {
            SetButton(defaultButtons[i].GetComponent<Button>());
        }
        else
        {
            SetRandomButton();
        }
      //  print("Setting default button");

    }

    public void SetRandomButton()
    {
       // print("setting random button function");
        Button[] allButtons = FindObjectsOfType<Button>(true);
        List<Button> validButtons = allButtons
            .Where(b => b.gameObject.activeInHierarchy && b.enabled && b.interactable)
            .ToList();
        validButtonsStore = validButtons;
        allButtonsStore = allButtons;
        if (validButtons.Count == 0)
        {
            Debug.LogWarning("No valid buttons found to select.");
            return;
        }
        int i = 0;
        for (i = 0; i < validButtons.Count; i++)
        {
            bool failed = false;
            // Check parent CanvasGroups for interactable = false
            Transform current = validButtons[i].transform;
            while (current != null)
            {
                CanvasGroup cg = current.GetComponent<CanvasGroup>();
                if (cg != null && !cg.interactable)
                {
                  //  print("Null or parent is not interactable");

                    break; // go further in for
                }

                current = current.parent;
            }

            if (failed)
            {
                continue;
            }
            else
            {
                break;
            }
        }



        Button randomButton = validButtons[i];

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(randomButton.gameObject);
        currentButton = randomButton.gameObject;
    }



    public void SetDefaultButton()
    {
       // print("In default button function");
        currentButton = defaultButtons[0];
        EventSystem.current.SetSelectedGameObject(defaultButtons[0]);
    }    
    
    public void SetButton(Button buttonToSet)
    {
        EventSystem.current.SetSelectedGameObject(buttonToSet.gameObject);
        currentButton = buttonToSet.gameObject;
    }

}
