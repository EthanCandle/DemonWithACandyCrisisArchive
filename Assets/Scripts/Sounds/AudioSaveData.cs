using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSaveData
{
    public int sfxVolume, musicVolume;
    public float controllerSensitivity;
    public bool isSFXMuted, isMusicMuted, isParticlesMuted, isShadersMuted, isMainMenuCandyMuted;
}
