using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFXOnHover : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private static float lastPlayTime;
    private static float minDelayBetweenPlays = 0.05f;


    public void OnPointerEnter(PointerEventData eventData)
    {
      //  print($"{Time.unscaledTime}, {lastPlayTime}");

        if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
        {//
          //  print("OnPointerEnter Triggered");
            // buttonHoverSFX
           // print(FindObjectOfType<AudioManager>().buttonHoverSFX.nameOfSound);
            FindObjectOfType<AudioManager>().PlaySoundInstantiate(FindObjectOfType<AudioManager>().buttonHoverSFX);
            lastPlayTime = Time.unscaledTime;
        }
    }

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    print($"{Time.unscaledTime}, {lastPlayTime}");

    //    if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
    //    {
    //        print("OnPointerEnter Triggered");
    //        // buttonHoverSFX
    //        print(FindObjectOfType<AudioManager>().buttonHoverSFX.nameOfSound);
    //        FindObjectOfType<AudioManager>().PlaySoundInstantiate(FindObjectOfType<AudioManager>().buttonHoverSFX);
    //        lastPlayTime = Time.unscaledTime;
    //    }
    //}    
    public void OnPointerClick(PointerEventData eventData)
    {
        //print($"{Time.unscaledTime}, {lastPlayTime}");

        if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
        {
          //  print("OnPointerEnter Triggered");
            // buttonHoverSFX
            //print(FindObjectOfType<AudioManager>().buttonHoverSFX.nameOfSound);
            FindObjectOfType<AudioManager>().PlaySoundInstantiate(FindObjectOfType<AudioManager>().buttonHoverSFX);
            lastPlayTime = Time.unscaledTime;
        }
    }
}
