using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSFXOnHover : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IMoveHandler
{
    private static float lastPlayTime;
    private static float minDelayBetweenPlays = 0.05f;

    public AudioManager audioManager;
    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      //  print($"{Time.unscaledTime}, {lastPlayTime}");

        if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
        {//
         // print("OnPointerEnter Triggered");
         // buttonHoverSFX
         // print(FindObjectOfType<AudioManager>().buttonHoverSFX.nameOfSound);

            audioManager.PlaySoundInstantiate(audioManager.buttonHoverSFX);
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


    public void OnPointerUp(PointerEventData eventData)
    {
        //print($"{Time.unscaledTime}, {lastPlayTime}");

        if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
        {
            //  print("OnPointerEnter Triggered");
            // buttonHoverSFX
            //print(FindObjectOfType<AudioManager>().buttonHoverSFX.nameOfSound);
            audioManager.PlaySoundInstantiate(audioManager.buttonHoverSFX);
            lastPlayTime = Time.unscaledTime;
        }
    }
    //public void OnSelect(BaseEventData eventData)
    //{
    //    if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
    //    {
    //        //  print("OnPointerEnter Triggered");
    //        // buttonHoverSFX
    //        //print(FindObjectOfType<AudioManager>().buttonHoverSFX.nameOfSound);
    //        audioManager.PlaySoundInstantiate(audioManager.buttonHoverSFX);
    //        lastPlayTime = Time.unscaledTime;
    //    }
    //}

    public void OnMove(AxisEventData eventData)
    {
        //if (GetComponent<Slider>() == null)
        //    return;

       // if (eventData.moveDir == MoveDirection.Left || eventData.moveDir == MoveDirection.Right)
        {
            if (Time.unscaledTime - lastPlayTime > minDelayBetweenPlays)
            {
                audioManager.PlaySoundInstantiate(audioManager.buttonHoverSFX);
                lastPlayTime = Time.unscaledTime;
            }
        }
    }



}
