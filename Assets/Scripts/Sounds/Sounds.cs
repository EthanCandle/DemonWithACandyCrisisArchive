using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Audio/Sound")]
public class Sound : ScriptableObject
{
    public string nameOfSound;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(0f, 1f)]
    public float volumeOriginal = 1;
    [Range(-3, 3)]
    public float pitch = 1;

    public bool loop;

   public AudioMixerGroup audioMixerGroup;

    [HideInInspector]
    public AudioSource source;



}
