using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSetObjectActive : MonoBehaviour
{
    public GameObject objectToSet;
    // Start is called before the first frame update
    void Start()
    {
        objectToSet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetObjectActive()
    {
        objectToSet.SetActive(true);
    }

}
