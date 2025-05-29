using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePlayOnLand : MonoBehaviour
{
    public bool hasPlayed = false;
    // this will be attached to the platform
    public float noteDuration; // 0.25 for quarter notes, 1 is whole notes, 0.125 eight notes
    public Sound soundToPlay;
    public void SetSound(Sound soundToSet)
    {
        soundToPlay = soundToSet;
    }

    public void PlaySound()
    {
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(soundToPlay, noteDuration);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlaySound();
            hasPlayed = true;
            
        }
    }

}
