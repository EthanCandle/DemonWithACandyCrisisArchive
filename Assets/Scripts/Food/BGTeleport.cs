using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGTeleport : MonoBehaviour
{

   // public GameObject particleOnTeleport;
    public GameObject objectToAppear;

    public BoxGirlFunctions functions;

    public GameObject modelOfSelf;

    private void Start()
    {
        functions = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BoxGirlFunctions>();
        if(objectToAppear != null)
        {
            objectToAppear.SetActive(false);
        }

    }
    public void Teleport()
    {
        functions.Teleport(modelOfSelf, objectToAppear);

    }

    public void TurnOffModel()
    {
        // turns off the object that has collision and the script (should be at the end of a teleport
        gameObject.SetActive(false);
    }

}
