using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Note", menuName = "Note")]
public class NoteData : ScriptableObject
{
    public string valueOfNote;
    public Sound soundClip;
    public float yPosition; // y relative to the usual placement, it is actually the x axis
}
