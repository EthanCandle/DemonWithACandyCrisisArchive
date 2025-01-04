using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndLevel : MonoBehaviour
{
    public LevelTransitionManager levelTransManager;
    // Start is called before the first frame update
    void Start()
    {
        levelTransManager = GameObject.FindGameObjectWithTag("Transition").GetComponent<LevelTransitionManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("End"))
        {
            levelTransManager.EndGame();
        }       
    }

}
