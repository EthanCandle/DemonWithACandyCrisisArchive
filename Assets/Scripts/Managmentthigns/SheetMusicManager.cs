using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SheetMusicManager : MonoBehaviour
{
    // notes downloaded from: https://www.reddit.com/r/piano/comments/3u6ke7/heres_some_midi_and_mp3_files_for_individual/
    // a-3 means a#, which is bflat


    public float zOffSet = 12.5f; // amount per note to adjust
    public float zPositionCurrent = 0; //
    public SheetMusicObject musicObject;
    public GameObject noteEighth, noteQuarter, noteHalf, noteWhole;


    GameObject noteSpawnedCurrent;
    NotePlayOnLand noteScriptCurrent;

    public NoteData clipF, clipE, clipD, clipC, clipB, clipA, clipG;

    public Dictionary<string, NoteData> baseNotes = new Dictionary<string, NoteData>();

    void Awake()
    {
        baseNotes.Add("f", clipF);
        baseNotes.Add("e", clipE);
        baseNotes.Add("d", clipD);
        baseNotes.Add("c", clipC);
        baseNotes.Add("b", clipB);
        baseNotes.Add("a", clipA);
        baseNotes.Add("g", clipG);

        // Put the clips in order from top to bottom (F to G)
        string[] noteOrder = { "f", "e", "d", "c", "b", "a", "g" };
        NoteData[] noteObjects = { clipF, clipE, clipD, clipC, clipB, clipA, clipG };

        float startingY = -50f;
        float offset = 12.5f;

        for (int i = 0; i < noteOrder.Length; i++)
        {
            noteObjects[i].yPosition = startingY + (i * offset);
            baseNotes[noteOrder[i]] = noteObjects[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnSheetMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNote(string noteToSpawnString, string noteLengthString)
    {
        NoteData noteDataCurrent = baseNotes[noteToSpawnString];
        Vector3 spawnOffSet = new Vector3(noteDataCurrent.yPosition, 0, zPositionCurrent);
        GameObject noteGoingToSpawn = noteWhole;
        print(noteLengthString);
        switch (noteLengthString)
        {


            case ("1"):
            {
                noteGoingToSpawn = noteWhole;
                    zPositionCurrent += zOffSet * 9;
                    break;
            }
            case ("2"):
                {
                    noteGoingToSpawn = noteHalf;
                    zPositionCurrent += zOffSet * 6;
                    break;

                }
            case ("4"):
                {
                    noteGoingToSpawn = noteQuarter;
                    zPositionCurrent += zOffSet * 3;
                    break;
                }
            case ("8"):
                {
                    noteGoingToSpawn = noteEighth;
                    zPositionCurrent += zOffSet * 1.5f;
                    break;
                }
            default:
                {
                    print($"No note selected {noteLengthString == "4"}");
                    break;
                }

        }
        noteSpawnedCurrent = Instantiate(noteGoingToSpawn, spawnOffSet, noteGoingToSpawn.transform.rotation);
        noteScriptCurrent = noteSpawnedCurrent.GetComponent<NotePlayOnLand>();
        noteScriptCurrent.SetSound(noteDataCurrent.soundClip);
    }

    public void SpawnSheetMusic()
    {

        for(int i = 0; i < musicObject.notes.Length; i++)
        {
            string noteToSpawn = musicObject.notes[i].ToString();
            i++;
            string noteLength = musicObject.notes[i].ToString();
            i++;
            print($"1: {noteToSpawn}, 2: {noteLength}");
            SpawnNote(noteToSpawn, noteLength);
        }
    }

}
