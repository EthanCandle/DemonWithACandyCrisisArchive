using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool isInOptions = false;
    public AudioManager audioManager;
    public int volume;
    public Slider volumeSliderSFX, volumeSliderMusic, controllerSensitivitySlider;

    public bool isMutedSFX = false, isMutedMusic = false;
    public GameObject muteObjectSFX, unMuteObjectSFX, muteObjectMusic, unMuteObjectMusic;
    public Animator optionsAnimator;
    public CanvasGroup menuToDeactiveOnSummon, canvasGroupLocal;
    public Sound summonSound, deSummonSound;
    public ReselectDefaultButton reselectButtonScript;

    public Button settingsDefaultButton;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        if (audioManager.audioDataLocal.isMusicMuted)
        {
            print("Music muted");
            MuteMusic();
            audioManager.CallDelayFrameMusic();
        }
        if (audioManager.audioDataLocal.isSFXMuted)
        {
            print("Music muted");
            MuteSFX();
            audioManager.CallDelayFrameSFX();
        }

        CallDelayStartStuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallDelayStartStuff()
    {
        StartCoroutine(DelayStartStuff());
    }

    public IEnumerator DelayStartStuff()
    {
        yield return null;
        SetSliderOnStartSFX();
        SetSliderOnStartMusic();
        SetSliderOnStartController();
    }
    public void SetSliderOnStartSFX()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        volumeSliderSFX.value = audioManager.audioDataLocal.sfxVolume;
    }
    public void SetSliderOnStartMusic()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        print("Set slide on music");
        volumeSliderMusic.value = audioManager.audioDataLocal.musicVolume;
    }
    public void SetSliderOnStartController()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        print("Set slide on controller");
        controllerSensitivitySlider.value = audioManager.audioDataLocal.controllerSensitivity;
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

    public void ChangeControllerSensitivity(Slider slider)
    {
        // print((int)volumeSlider.value);

        audioManager.SetControllerSensitivity((float)slider.value);
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

        if (muteObjectSFX != null)
            muteObjectSFX.SetActive(true);

        if (unMuteObjectSFX!= null)
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
        if(muteObjectMusic != null)
        muteObjectMusic.SetActive(true);

        if(unMuteObjectMusic != null)
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

    public void ToggleOptionsMenu(Button button)
    {
        if (isInOptions)
        {
            DesummonOptionsMenu();
        }
        else
        {
            SummonOptionsMenu(button);
        }
    }

    // called by buttons, 
    public void SummonOptionsMenu(Button button)
    {
        if (!isInOptions)
        {
            optionsAnimator.SetTrigger("Move");
        }
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);
        // called by options button
        isInOptions = true;
        optionsAnimator.SetTrigger("Move");
        menuToDeactiveOnSummon.interactable = false;
        canvasGroupLocal.interactable = true;
       // ReselectButton();

        ReselectDefaultButton.instance.SetPreviousButton(button);
        ReselectDefaultButton.instance.SetButton(settingsDefaultButton);
    }

    public void ReselectButton()
    {
        StartCoroutine(DelayFrame());
    }

    public IEnumerator DelayFrame()
    {
        yield return null;
        reselectButtonScript.SelectRandomButton();
    }
    public void DesummonOptionsMenu()
    {
        if (isInOptions)
        {
            optionsAnimator.SetTrigger("Move");
        }
        print("options menu pause");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);
        // called by back button
        isInOptions = false;

        menuToDeactiveOnSummon.interactable = true;
        canvasGroupLocal.interactable = false;

        ReselectDefaultButton.instance.GoBackToPreviousButton();
    }


}
