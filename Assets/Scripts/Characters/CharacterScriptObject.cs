using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Avatar", menuName = "ScriptableObjects/Avatar")]
public class CharacterScriptObject : ScriptableObject
{
    // image, name, color/font for text
    public Texture avatarTexture;
    public string nameOfSelf;
    public List<string> animationsPlayable;
    public string animationTransitionName;
    public float delayWithinTransition;
}
