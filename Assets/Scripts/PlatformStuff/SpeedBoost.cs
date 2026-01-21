using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public Sound speedSFX;
    public float speedStrength = 1.5f;
	public float lastPlayTime;
	private float minDelayBetweenPlays = 3f;
	public AudioManager audioManager;
	public void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
	}
	private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
			lastPlayTime = 0;
			FillPlayer(other.gameObject);
			SpeedPlayer(other.gameObject);
        }
    }

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			
			print("trigger stay");
			SpeedPlayer(other.gameObject);
		}
	}

	private void OnCollisionStay(Collision other)
    {
		if (other.gameObject.CompareTag("Player"))
		{
			print("collision stay");
			SpeedPlayer(other.gameObject);
		}
	}

    public void SpeedPlayer(GameObject player)
    {
		player.gameObject.GetComponent<PlayerController>().SetTempSpeed(speedStrength);
		PlaySound();

	}

	public void FillPlayer(GameObject player)
	{
		player.gameObject.GetComponent<PlayerController>().FullyChargeDashMeter();

	}


	public void PlaySound()
	{
		if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
		{
			audioManager.PlaySoundInstantiate(speedSFX);
			lastPlayTime = Time.unscaledTime;
		}
	}

}
