using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct DialogueData
{
    public CharacterScriptObject dialogueAsset;  // holds name and avatar
    public string dialogueText;             // Dialogue line
    public UnityEvent functionToCall;       // Function reference
    public Animator objectAnimator;         // Animator reference from the GameObject
    public string AnimationToPlayName;
    public bool shouldTransitionToThisAnimation; // should it play the jump animation
    public bool shouldKillSelfAfterThis;
}
