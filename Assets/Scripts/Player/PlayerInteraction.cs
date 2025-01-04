using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction : MonoBehaviour
{
    public StarterAssetsInputs _input;
    public bool canInteract = true;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();




    }
    /*
     * Generic template to create a interact function only
     * on press and not hold or release
        if (_input.interact && canInteract)
        {
            canInteract = false;
            print("DID it"); // replace this with spawning something or idk
            return;
        }

        if (!_input.interact)
        {
            canInteract = true;
            return;
        }
    */


// Update is called once per frame
    void Update()
    {
        InteractWithCheck();


    }

    public void OnTriggerStay(Collider other)
    {
        // when player enters a trigger, check if they want to interact
        // and then call the function on the other object to interact
        InteractWithCheck();
    }


    public void InteractWithCheck()
    {
        if (_input.interact && canInteract)
        {
            _input.interact = false;
            //canInteract = false;
            
            print("DID it"); // replace this with spawning something or idk
            return;
        }

        if (!_input.interact)
        {
           // canInteract = true;

            return;
        }


    }


}
