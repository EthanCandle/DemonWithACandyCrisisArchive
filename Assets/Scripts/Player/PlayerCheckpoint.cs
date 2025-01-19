using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    public GameObject currentCP;
    public Vector3 currentCheckPointLocation;

    public PlayerDeath playerDeath;
    public float waitTimeWhenRespawn;

    // Start is called before the first frame update
    void Start()
    {
        playerDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToCheckPoint()
    {
        // move player object to the CP's location and prevent movement for a certain amount of time

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            SetCheckPoint(other.gameObject);
        }
    }

    public void SetCheckPoint(GameObject otherObj)
    {
        currentCP = otherObj.transform.GetChild(0).gameObject;
        // gets the child object
        currentCheckPointLocation = currentCP.transform.position;//currentCP.transform.position;
        playerDeath.SetPlayerRespawnPoint(currentCheckPointLocation, currentCP);
        //print($"")
    }
}
