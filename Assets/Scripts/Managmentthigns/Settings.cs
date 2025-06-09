using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool isInOptions = false;
    public AudioManager audioManager;
    public int volume;
    public Slider volumeSliderSFX, volumeSliderMusic;

    public bool isMutedSFX = false, isMutedMusic = false;
    public GameObject muteObjectSFX, unMuteObjectSFX, muteObjectMusic, unMuteObjectMusic;
    public Animator optionsAnimator;
    public CanvasGroup menuToDeactiveOnSummon;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        SetSliderOnStartSFX();
        SetSliderOnStartMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSliderOnStartSFX()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        volumeSliderSFX.value = audioManager.currentVolumeSFX;
    }
    public void SetSliderOnStartMusic()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        volumeSliderMusic.value = audioManager.currentVolumeMusic;
    }

    public void ChangeVolumeSFX(Slider slider)
    {
       // print((int)volumeSlider.value);
        audioManager.SetVolumeSFX((int)slider.value);
        UnMuteSFX(); // just to remove the mute symbol
    }
    public void ChangeVolumeMusic(Slider slider)
    {
        // print((int)volumeSlider.value);
        audioManager.SetVolumeMusic((int)slider.value);
        UnMuteMusic(); // just to remove the mute symbol
    }
    public void ChangeMuteSFX()
    {
        if (isMutedSFX)
        {
            UnMuteSFX();
        }
        else
        {
            MuteSFX();
        }        
    }
    public void ChangeMuteMusic()
    {

        if (isMutedMusic)
        {
            UnMuteMusic();
        }
        else
        {
            MuteMusic();
        }
    }
    public void MuteSFX()
    {
        isMutedSFX = true;
        // called by button
        audioManager.MuteVolumeSFX();
        muteObjectSFX.SetActive(true);
        unMuteObjectSFX.SetActive(false);
    }

    public void UnMuteSFX()
    {
        isMutedSFX = false;
        // called by button
        audioManager.UnMuteVolumeSFX();
        muteObjectSFX.SetActive(false);
        unMuteObjectSFX.SetActive(true);

    }

    public void MuteMusic()
    {
        isMutedMusic = true;
        // called by button
        audioManager.MuteVolumeMusic();
        muteObjectMusic.SetActive(true);
        unMuteObjectMusic.SetActive(false);
    }

    public void UnMuteMusic()
    {
        isMutedMusic = false;
        // called by button
        audioManager.UnMuteVolumeMusic();
        muteObjectMusic.SetActive(false);
        unMuteObjectMusic.SetActive(true);

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
