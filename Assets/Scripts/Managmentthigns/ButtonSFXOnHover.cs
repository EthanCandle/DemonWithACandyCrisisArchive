using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFXOnHover : MonoBehaviour, IPointerEnterHandler
{
    private static float lastPlayTime;
    private static float minDelayBetweenPlays = 0.05f;


    public void OnPointerEnter(PointerEventData eventData)
    {
        print("OnPointerEnter Triggered");
        if (Time.time - lastPlayTime > minDelayBetweenPlays)
        {
            // buttonHoverSFX
            print(FindObjectOfType<AudioManager>().buttonHoverSFX.nameOfSound);
            FindObjectOfType<AudioManager>().PlaySoundInstantiate(FindObjectOfType<AudioManager>().buttonHoverSFX);
            lastPlayTime = Time.time;
        }
    }
}
