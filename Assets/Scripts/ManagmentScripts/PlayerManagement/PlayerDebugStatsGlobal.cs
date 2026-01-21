using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDebugStatsGlobal
{
    // has bean the game before
    // total candy collected, total jumps, deaths
    // fastest time to complete the game/each level

    // local is one run. Should just derive from this, but should be its own class
    public bool hasBeatenGame = false, // maybe change main menu to reflect this
        isInMainRun = true; //
    public int levelCurrentlyOnMainRun = 0;
    public int amountPlayerJumps, amountPlayerDashes, amountPlayerDies, amountPlayerCandy;
    public float fastestTimeToCompleteGame, currentTimeToCompleteGame; // this is the whole game
    public List<float> fastestLevelTimes, currentLevelTimes; // index 1 is level 1's fastest time
}
