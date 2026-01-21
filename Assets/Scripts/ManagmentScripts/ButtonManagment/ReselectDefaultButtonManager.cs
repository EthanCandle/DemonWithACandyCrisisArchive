using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReselectDefaultButtonManager : MonoBehaviour
{
    // attached in each scene to set which button sohuld be the default

    public Button defaultButtonForScene;

    // Start is called before the first frame update
    void Start()
    {
        ReselectDefaultButton.instance.SetAndGoToButton(defaultButtonForScene);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
