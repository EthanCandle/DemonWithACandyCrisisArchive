using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectInput : MonoBehaviour
{
    public GameObject childToToggle;
    public bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
		childToToggle.SetActive(isOn);
	}

    // Update is called once per frame
    void Update()
    {

	}


    public void ToggleChild()
	{
		isOn = !isOn;
		childToToggle.SetActive(isOn);
	}

    public void SetChild(bool state)
    {
        isOn = state;
		childToToggle.SetActive(isOn);
	}
}
