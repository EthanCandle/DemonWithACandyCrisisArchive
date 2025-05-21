using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    public GameObject currentCP;
    public Vector3 currentCheckPointLocation;

    public PlayerDeath playerDeath;
    public float waitTimeWhenRespawn;

    public Animator checkPointAnimator;

    public Sound soundHittingCheckPoint;

    public CheckPointManager checkPointManager;
    // Start is called before the first frame update
    void Start()
    {
        playerDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>();
        checkPointManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CheckPointManager>();
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
            // bail out if we are already 
            if(other.transform.parent.gameObject == currentCP)
            {
                return;
            }
            SetCheckPoint(other.gameObject);
        }
    }

    public void SetCheckPoint(GameObject otherObj)
    {
       // a checkpoint is a cube with a hitbox child. The cube parent is where the player should be placed.

        // safeguard for the first time the player touches the checkpoint
        if(checkPointAnimator != null)
        {
            checkPointAnimator.SetBool("IsSet", false);
        }

        currentCP = otherObj.transform.parent.gameObject;
        // need to get the current cps animator and set it to go down
        checkPointAnimator = currentCP.GetComponentInChildren<Animator>();
        checkPointAnimator.SetBool("IsSet", true);

        currentCheckPointLocation = currentCP.transform.position;
        playerDeath.SetPlayerRespawnPoint(currentCheckPointLocation, currentCP);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(soundHittingCheckPoint);
        // need to get its animator and play the animation for it to sprout up
        //print($"")
    }
}
