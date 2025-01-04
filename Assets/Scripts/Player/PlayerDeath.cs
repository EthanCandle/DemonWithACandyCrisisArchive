using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public float timeUntilRespawn = 5.0f;


    public GameObject deathParticles;
    public Sound deathSFX;
    public PlayerController playerController;
    public Vector3 playerRespawnPoint; // should be brought by playerCheckPoint

    public GameObject playerCamera, playerFakeCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerRespawnPoint = playerController.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerRespawnPoint(Vector3 positionToSetTo)
    {
        
        playerRespawnPoint = positionToSetTo;
    }

    // should be called by player health
    public void PlayerShouldDeath()
    {
        // play sound effect, player animation, freeze movement/input
        playerController.LosePlayerControl();
        StartCoroutine(RespawnDelay());
        SetFakeCamera();
    }

    public IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(timeUntilRespawn);
        Respawn();
    }

    public void Respawn()
    {
        // put player back, particle again on respawn, regive movement/input
        playerController.GainPlayerControl();
        playerController.gameObject.transform.position = playerRespawnPoint;
        EndFakeCamera();
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
        if (other.CompareTag("Death"))
        {
            PlayerShouldDeath();
        }
    }
}
