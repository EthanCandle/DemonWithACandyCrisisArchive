using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGActivateAnimationTrigger : MonoBehaviour
{
    public Animator animatorToTrigger;
    public string nameOfTrigger;
    public Sound soundToPlay;
    public void TriggerTrigger()
    {
        animatorToTrigger.SetTrigger(nameOfTrigger);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(soundToPlay);
    }

}
