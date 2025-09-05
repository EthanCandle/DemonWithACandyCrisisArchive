using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndLevel : MonoBehaviour
{
    public LevelTransitionManager levelTransManager;
    public bool canTriggerEnding = true;
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
        if (other.gameObject.CompareTag("End") && canTriggerEnding)
        {
            canTriggerEnding = false;
            print("triggered end level");
            levelTransManager.EndGame();
        }       
    }

}
