using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationPopUpManager : MonoBehaviour
{
    public ConfirmationPopUP confirmationCurrentlyUp;
    public bool isPopUp = false;
    // Start is called before the first frame update

    public void SetPopUp(ConfirmationPopUP popUp)
    {
        confirmationCurrentlyUp = popUp;
        isPopUp = true;
    }

    public void DestroyPopUp()
    {
        confirmationCurrentlyUp.CloseOnNo();
    }

}
