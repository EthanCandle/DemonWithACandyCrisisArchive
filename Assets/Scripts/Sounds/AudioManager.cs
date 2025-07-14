using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    // FindObjectOfType<AudioManager>().PlaySoundInstantiate(deathSFX);

    public int currentVolumeSFX = 100, currentVolumeMusic = 100;
    public bool isSFXMuted, isMusicMuted;
    public Sound[] mainMenuMusic;
    public Sound[] gamePlayMusic;
    public Sound[] sounds;
    public Sound buttonHoverSFX;
    public static AudioManager instance;
    public AudioSource songCurrentlyPlaying;

    public int amountOfSoundsSoFar = 0;

    public bool shouldFadeIn = false, shouldFadeOut = false;
    public float fadeInOutTime = 1.0f;
    public float fadeInOutSpeed = 0.5f;
    public float songVolumeMax; // this is the music's current volume before transitioning

    public List<AudioMixerGroup> audioMixerGroupMusics, audioMixerGroupSFXs;


    public bool inMainMenuFirstTime = true;

    public string volumeData = Application.dataPath + "/volumeData.txt";
    public AudioSaveData audioDataLocal;
    public Settings settingScript;
    public GameManager gm;
    // Start is called before the first frame update
    void Awake()
    {
        //   print("Gett setting in audiomanager");


        // create self if not already in
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //AdjustVolumeSFX(currentVolumeSFX);
        //AdjustVolumeMusic(currentVolumeMusic);
        settingScript = GameObject.FindGameObjectWithTag("Options").GetComponent<Settings>();
        LoadData();
    }
    void Start()
    {
        settingScript = GameObject.FindGameObjectWithTag("Options").GetComponent<Settings>();
        GameObject gmObject = GameObject.FindGameObjectWithTag("GameManager");

        if (gmObject != null && gmObject.TryGetComponent<GameManager>(out gm))
        {
            // gm is now assigned and usable
        }
    }
    public Sound FindSound(string name)
    {
        // goes through the sounds array
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }


    public void PlaySoundInstantiate(Sound sourceSound)
    {
        if(sourceSound == null || isSFXMuted)
        {
            return;
        }
       // print(sourceSound.name);
        // plays a sound by creating an empty object with the sound attached and deletes it when it ends
        amountOfSoundsSoFar++;

        // FindObjectOfType<AudioManager>().PlaySoundInstantiate(deathSFX);
        // use the var:     public Sound deathSFX;

        // creates the object
        GameObject newGameObject = new GameObject($"Sound Holder ({amountOfSoundsSoFar})", typeof(AudioSource));

        // addes the audiosource and sets it
        sourceSound.source = newGameObject.GetComponent<AudioSource>();

        sourceSound.source.clip = sourceSound.clip;

        sourceSound.source.volume =  sourceSound.volumeOriginal; // currentVolume / 100.0f *
        sourceSound.source.pitch = sourceSound.pitch;
        sourceSound.source.loop = sourceSound.loop;
        sourceSound.source.outputAudioMixerGroup = sourceSound.audioMixerGroup;
        // plays teh sound
        sourceSound.source.Play();

        // deletes objects after finishing (might mess up if pitch is different, maybe multiply or divide by pitch?)
        Destroy(newGameObject, sourceSound.clip.length / sourceSound.pitch);
    }

    public void PlaySoundInstantiate(Sound sourceSound, float timeToPlayRatio)
    {
        if (sourceSound == null || isSFXMuted)
        {
            return;
        }
        // print(sourceSound.name);
        // plays a sound by creating an empty object with the sound attached and deletes it when it ends
        amountOfSoundsSoFar++;

        // FindObjectOfType<AudioManager>().PlaySoundInstantiate(deathSFX);
        // use the var:     public Sound deathSFX;

        // creates the object
        GameObject newGameObject = new GameObject($"Sound Holder ({amountOfSoundsSoFar})", typeof(AudioSource));

        // addes the audiosource and sets it
        sourceSound.source = newGameObject.GetComponent<AudioSource>();

        sourceSound.source.clip = sourceSound.clip;

        sourceSound.source.volume =  sourceSound.volumeOriginal; // currentVolume / 100.0f *
        sourceSound.source.pitch = sourceSound.pitch;
        sourceSound.source.loop = sourceSound.loop;
        sourceSound.source.outputAudioMixerGroup = sourceSound.audioMixerGroup;
        // plays teh sound
        sourceSound.source.Play();

        // deletes objects after finishing (might mess up if pitch is different, maybe multiply or divide by pitch?)
        Destroy(newGameObject, sourceSound.clip.length / sourceSound.pitch * timeToPlayRatio);
    }

    public void PlaySoundInstantiate(string nameOfSound)
    {
        // plays a sound by creating an empty object with the sound attached and deletes it when it ends
        Sound sourceSound = FindSound(nameOfSound);

        // call using this
        // FindObjectOfType<AudioManager>().PlaySoundInstantiate(deathSFX);
  

        // creates empty object with an audioSource on it
        GameObject newGameObject = new GameObject($"Sound Holder (1)", typeof(AudioSource));
        //AddComponent
        sourceSound.source = newGameObject.GetComponent<AudioSource>();

        sourceSound.source.clip = sourceSound.clip;

        sourceSound.source.volume = sourceSound.volume;
        sourceSound.source.pitch = sourceSound.pitch;
        sourceSound.source.loop = sourceSound.loop;

        // plays the sound
        sourceSound.source.Play();

        // deletes objects after finishing (might mess up if pitch is different, maybe multiply or divide by pitch?)
        Destroy(newGameObject, sourceSound.clip.length / sourceSound.pitch);
    }


    private void Update()
    {
        if (shouldFadeOut)
        {
            songCurrentlyPlaying.volume -= fadeInOutSpeed * Time.unscaledDeltaTime;
            if (songCurrentlyPlaying.volume <= 0)
            {
               // print("Fully faded");
                shouldFadeOut = false;
                songCurrentlyPlaying.Stop();
            }
        }

        if (shouldFadeIn)
        {
            songCurrentlyPlaying.volume += fadeInOutSpeed / 5 * Time.unscaledDeltaTime;
            if (songCurrentlyPlaying.volume >= songVolumeMax)
            {

                shouldFadeIn = false;
                songCurrentlyPlaying.volume = songVolumeMax;
            }
        }

    }

    public void GetTransitionSpeed()
    {
       // print(songVolumeMax);
        songVolumeMax = songCurrentlyPlaying.volume;
        fadeInOutSpeed = songCurrentlyPlaying.volume / fadeInOutTime;
       // print(songVolumeMax);
    }

    public void FadeOut()
    {
        // this should only be called by the transition manager
        shouldFadeOut = true;
        GetTransitionSpeed();

    }
    public void FadeIn()
    {
        // this should only be called by the transition manager
        // sets the fade in to true, gets teh new transition speed based on the volume of the song, mutes teh song so it comes back in teh fade
        PlayLevelMusic();
        shouldFadeIn = true;
        GetTransitionSpeed();
        songCurrentlyPlaying.volume = 0;
    }
    // FindObjectOfType<AudioManager>().Play("BombDeport");

    public void PlaySFX(string name)
    {
        //  FindObjectOfType<AudioManager>().PlaySFX("BombDeport");

        // goes through the sounds array
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();

    }

    public void PlayMainMenuTheme(string name)
    {
        //  FindObjectOfType<AudioManager>().PlayMainMenuTheme("BombDeport");

        StopCurrentSong();

        // goes through the music array
        Sound s = Array.Find(mainMenuMusic, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
        songCurrentlyPlaying = s.source;

    }

    public void PlayLevelMusic()
    {
        Sound sourceSound = gamePlayMusic[SceneManager.GetActiveScene().buildIndex];

        if (sourceSound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        // print(sourceSound.name);
        // plays a sound by creating an empty object with the sound attached and deletes it when it ends
        amountOfSoundsSoFar++;

        // FindObjectOfType<AudioManager>().PlaySoundInstantiate(deathSFX);
        // use the var:     public Sound deathSFX;

        // creates the object
        GameObject newGameObject = new GameObject($"Sound Holder ({amountOfSoundsSoFar})", typeof(AudioSource));

        // addes the audiosource and sets it
        sourceSound.source = newGameObject.GetComponent<AudioSource>();

        sourceSound.source.clip = sourceSound.clip;

        sourceSound.source.volume = sourceSound.volumeOriginal; //currentVolume / 100.0f * 
        sourceSound.source.pitch = sourceSound.pitch;
        sourceSound.source.loop = sourceSound.loop;
        sourceSound.source.outputAudioMixerGroup = sourceSound.audioMixerGroup;
        // plays teh sound
        sourceSound.source.Play();

        songCurrentlyPlaying = sourceSound.source;


    }

    public void StopCurrentSong()
    {
        if (songCurrentlyPlaying == null)
        {
            return;
        }

        songCurrentlyPlaying.Stop();
    }

    public void PauseCurrentSong()
    {
        if (songCurrentlyPlaying == null)
        {
            return;
        }

        songCurrentlyPlaying.Pause();
    }

    public void UnPauseCurrentSong()
    {
        if (songCurrentlyPlaying == null)
        {
            return;
        }

        songCurrentlyPlaying.UnPause();
    }

    public void PlayGamePlayMusic(string name)
    {
        //  FindObjectOfType<AudioManager>().PlayGamePlayMusic("Ambient");

        StopCurrentSong();
        // goes through the music array
        Sound s = Array.Find(gamePlayMusic, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
        songCurrentlyPlaying = s.source;
    }


    public void PlayRandomGamePlayMusic()
    {
        //   FindObjectOfType<AudioManager>().PlayRandomGamePlayMusic();

        // plays a random song from the music playlist
        // called when a song ends (need to implement)

        StopCurrentSong();

        Sound s = gamePlayMusic[UnityEngine.Random.Range(0, gamePlayMusic.Length)];

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
        songCurrentlyPlaying = s.source;
    }


    public void SetVolumeSFX(int amountToChange)
    {
        audioDataLocal.sfxVolume = amountToChange;
        audioDataLocal.isSFXMuted = false;
        AdjustVolumeSFX(audioDataLocal.sfxVolume);


    }

    public void MuteVolumeSFX()
    {
        audioDataLocal.isSFXMuted = true;
        DirectSetVolumeSFX(-800);


    }

    public void UnMuteVolumeSFX()
    {
        audioDataLocal.isSFXMuted = false;
        AdjustVolumeSFX(audioDataLocal.sfxVolume);


    }    
    
    public void AdjustVolumeSFX(float volume)
    {
        float dB = Mathf.Lerp(-20, 20f, volume / 100f);


        DirectSetVolumeSFX(dB);

    }

    public void DirectSetVolumeSFX(float volume)
    {
        for (int i = 0; i < audioMixerGroupSFXs.Count; i++)
        {
            audioMixerGroupSFXs[i].audioMixer.SetFloat("volume", volume);
        }
        SaveData();
    }


    public void SetVolumeMusic(int amountToChange)
    {
        audioDataLocal.musicVolume = amountToChange;
        audioDataLocal.isMusicMuted = false;

       // print($"Music muted is: {audioDataLocal.isMusicMuted}, volume: {audioDataLocal.musicVolume}");
        AdjustVolumeMusic(audioDataLocal.musicVolume);

    }

    public void MuteVolumeMusic()
    {
        audioDataLocal.isMusicMuted = true;
      //  print($"Music muted is: {audioDataLocal.isMusicMuted}");

        DirectSetVolumeMusic(-800);


    }

    public void UnMuteVolumeMusic()
    {
        audioDataLocal.isMusicMuted = false;
        //print($"Music muted is: {audioDataLocal.isMusicMuted}");

        AdjustVolumeMusic(audioDataLocal.musicVolume);


    }

    public void AdjustVolumeMusic(float volume)
    {
        float dB = Mathf.Lerp(-20f, 20f, volume / 100f);

      //  print(dB);
        DirectSetVolumeMusic(dB);

    }

    public void DirectSetVolumeMusic(float volume)
    {
       // print(volume);
        for (int i = 0; i < audioMixerGroupMusics.Count; i++)
        {
        //    print("Set music");
            audioMixerGroupMusics[i].audioMixer.SetFloat("volume", volume);
        //    print(volume);
        }

       // float f;
       // print(audioMixerGroupMusics[0].audioMixer.GetFloat("volume", out f));
       // print(f);
        SaveData();
    }

    public void SaveData()
    {
       // print(audioDataLocal.isMusicMuted);
        AudioSaveData audioSaveData;

        if (audioDataLocal == null)
        {
            audioSaveData = new AudioSaveData { sfxVolume = 50, musicVolume = 50, isSFXMuted = false, isMusicMuted = false, controllerSensitivity = 5 };
           // print("saving data is null");
        }
        else
        {
            audioSaveData = new AudioSaveData { sfxVolume = audioDataLocal.sfxVolume, musicVolume = audioDataLocal.musicVolume, isSFXMuted = audioDataLocal.isSFXMuted, isMusicMuted = audioDataLocal.isMusicMuted, controllerSensitivity = audioDataLocal.controllerSensitivity };

        }



        string json = JsonUtility.ToJson(audioSaveData);

        File.WriteAllText(volumeData, json);
    }

    public void LoadData()
    {
        if (!File.Exists(volumeData))
        {
            print("Save candyData didn't exist");
            // if we don't have a save yet, make one
            SaveData();
        }
        string saveString = File.ReadAllText(volumeData);

        audioDataLocal = JsonUtility.FromJson<AudioSaveData>(saveString);

        SetVolumeData();
        LoadControllerSensitivity();

    }

    public void SetVolumeData()
    {
      //  print("Set volume datat");
        if (audioDataLocal.isMusicMuted)
        {
            SetVolumeMusic(audioDataLocal.musicVolume);
            MuteVolumeMusic();
            CallDelayFrameMusic();
        }
        else
        {
            SetVolumeMusic(audioDataLocal.musicVolume);

        }

        if (audioDataLocal.isSFXMuted)
        {
            SetVolumeSFX(audioDataLocal.sfxVolume);
            MuteVolumeSFX();
            CallDelayFrameSFX();
        }
        else
        {
            SetVolumeSFX(audioDataLocal.sfxVolume);
        }
    }

    public void CallDelayFrameMusic()
    {
        StartCoroutine(DelayFrameMusic());
    }

    public void CallDelayFrameSFX()
    {
        StartCoroutine(DelayFrameSFX());
    }

    public IEnumerator DelayFrameMusic()
    {

        yield return null;
        yield return null;
        print("IN coroco");
        settingScript = GameObject.FindGameObjectWithTag("Options").GetComponent<Settings>();
        audioDataLocal.isMusicMuted = true;
        settingScript.MuteMusic();

    }   
    public IEnumerator DelayFrameSFX()
    {

        yield return null;
        yield return null;
        print("IN coroco");
        settingScript = GameObject.FindGameObjectWithTag("Options").GetComponent<Settings>();
        audioDataLocal.isSFXMuted = true;
        settingScript.MuteSFX();
    }

    public void SetControllerSensitivity(float amountToChange)
    {
        print("set snesitity");
        audioDataLocal.controllerSensitivity = amountToChange;
        SaveData();
        LoadControllerSensitivity();
    }

    public void LoadControllerSensitivity()
    {
        GameObject gmObject = GameObject.FindGameObjectWithTag("GameManager");

        if (gmObject != null && gmObject.TryGetComponent<GameManager>(out gm))
        {
            // gm is now assigned and usable
        }
        if (gm == null)
        {
            return;
        }
        print("loaded snesitity");
        print(gm);
        gm._input.sensitivity = audioDataLocal.controllerSensitivity;
        print(gm._input.sensitivity);
    }
}
