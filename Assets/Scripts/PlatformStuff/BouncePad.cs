using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public Sound boingSFX;
    public float bounceStength = 6;

    public  float lastPlayTime;
    public  float minDelayBetweenPlays = 0.05f;

    public AudioManager audioManager;
    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BouncePlayer(other.gameObject);
        }
    }   
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BouncePlayer(other.gameObject);
        }
    }

    public void BouncePlayer(GameObject objToBounce)
    {
        objToBounce.gameObject.GetComponent<PlayerController>().Bounce(bounceStength);
        PlaySound();
    }

    public void PlaySound()
    {
        if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
        {
            audioManager.PlaySoundInstantiate(boingSFX);
            lastPlayTime = Time.unscaledTime;
        }
    }
}
