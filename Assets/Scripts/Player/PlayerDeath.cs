using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public float timeUntilRespawn = 5.0f;
    public bool isDead = false;

    public GameObject deathParticles;
    public Sound deathSFX;
    public PlayerController playerController;
    public Vector3 playerRespawnPoint; // should be brought by playerCheckPoint
    public Quaternion playerRotationPoint;
    public GameObject playerCamera, playerFakeCamera, checkPointCurrent;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerRespawnPoint = playerController.gameObject.transform.position;
        playerRotationPoint = playerController.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //  print($"RespawnPoint:{playerRespawnPoint} Player is now: {playerController.gameObject.transform.position}");
       // print($"RespawnPoint:{playerRespawnPoint} Player is now: {playerController.gameObject.transform.position}");

    }

    public void SetPlayerRespawnPoint(Vector3 positionToSetTo, GameObject checkPointObj)
    {
        
        playerRespawnPoint = positionToSetTo;
        checkPointCurrent = checkPointObj;
        playerRotationPoint = checkPointCurrent.transform.rotation;
    }

    // should be called by player health
    public void PlayerShouldDeath()
    {
        // play sound effect, player animation, freeze movement/input
        isDead = true;
        playerController.SetDead(true);
        
        // play death sound
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deathSFX);
        playerController.LosePlayerControl();
        StartCoroutine(RespawnDelay());
        SetFakeCamera();
        PlayerDebugStatsGlobalManager.Instance.dataLocal.amountPlayerDies++;
    }

    public IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(timeUntilRespawn);
        Respawn();
    }

    public void Respawn()
    {
        // put player back, particle again on respawn, regive movement/input
        //StartCoroutine(GivePlayerControl());
        SetPlayerPosition();
        isDead = false; // removes being dead on this script
        playerController.SetDead(false); // sets the player's script to not dead
        EndFakeCamera(); // disables the fake camera
        playerController.GainPlayerControl();
        //print($"DEATH RespawnPoint:{playerRespawnPoint} Player is now: {playerController.gameObject.transform.position}");
    }


    public void SetPlayerPosition()
    {
        // puts player at last checkpoint interacted with
        SetPlayerPosition(playerRespawnPoint);
    }

    public void SetPlayerPosition(Vector3 placeToPutPlayer)
    {
        // moves player to set vector position
        playerController.SetCharacterController(false); // need it to be false to prevent gravity and other movements being applied and re-teleporting the player back to where they were
        playerController.gameObject.transform.position = placeToPutPlayer; // teleports player to cp
        playerController.SetCameraRotation(playerRotationPoint); // sets camera to cp's rotation
        playerController.SetCharacterController(true); // re enables the character controller so they can move
        playerController.SetYVelocityToZero();
    }
    public IEnumerator GivePlayerControl()
    {
        yield return new WaitForSeconds(0.5f);

        print($"RespawnPoint:{playerRespawnPoint} Player is now: {playerController.gameObject.transform.position}");

    }

    public void SetFakeCamera()
    {
        print("Fake");
        playerFakeCamera.transform.position = playerCamera.transform.position;
        playerFakeCamera.transform.rotation = playerCamera.transform.rotation;
        playerFakeCamera.SetActive(true);
    }

    public void EndFakeCamera()
    {
        print("End Fake");
        playerFakeCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death") && !isDead)
        {

            print("Die");
            PlayerShouldDeath();
        }
    }
}
