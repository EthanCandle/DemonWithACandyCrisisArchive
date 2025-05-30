using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public Sound speedSFX;
    public float speedStrength = 1.5f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpeedPlayer(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpeedPlayer(other.gameObject);
        }
    }

    public void SpeedPlayer(GameObject objToBounce)
    {
        objToBounce.gameObject.GetComponent<PlayerController>().SetTempSpeed(speedStrength);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(speedSFX);
    }
}
