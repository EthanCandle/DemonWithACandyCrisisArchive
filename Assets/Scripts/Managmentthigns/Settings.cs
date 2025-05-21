using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool isInOptions = false;
    public AudioManager audioManager;
    public int volume;
    public Slider volumeSlider;

    public bool isMuted = false;
    public GameObject muteObject, unMuteObject;
    public Animator optionsAnimator;
    public CanvasGroup menuToDeactiveOnSummon;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        SetSliderOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSliderOnStart()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        volumeSlider.value = audioManager.currentVolume;
    }


    public void ChangeVolume()
    {
       // print((int)volumeSlider.value);
        audioManager.SetVolume((int)volumeSlider.value);
        UnMute(); // just to remove the mute symbol
    }

    public void ChangeMute()
    {
        if (isMuted)
        {
            UnMute();
        }
        else
        {
            Mute();
        }
    }

    public void Mute()
    {
        isMuted = true;
        // called by button
        audioManager.MuteVolume();
        muteObject.SetActive(true);
        unMuteObject.SetActive(false);
    }

    public void UnMute()
    {
        isMuted = false;
        // called by button
        audioManager.UnMuteVolume();
        muteObject.SetActive(false);
        unMuteObject.SetActive(true);

    }

    public void ToggleOptionsMenu()
    {
        if (isInOptions)
        {
            DesummonOptionsMenu();
        }
        else
        {
            SummonOptionsMenu();
        }
    }

    // called by buttons, 
    public void SummonOptionsMenu()
    {
        if (!isInOptions)
        {
            optionsAnimator.SetTrigger("Move");
        }
        // called by options button
        isInOptions = true;
        optionsAnimator.SetTrigger("Move");
        menuToDeactiveOnSummon.interactable = false;
    }

    public void DesummonOptionsMenu()
    {
        if (isInOptions)
        {
            optionsAnimator.SetTrigger("Move");
        }
        // called by back button
        isInOptions = false;

        menuToDeactiveOnSummon.interactable = true;
    }


}
