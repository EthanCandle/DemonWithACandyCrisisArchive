using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDebugStatsGlobal
{
    public bool hasBeatenGame = false;
    public int amountPlayerJumps, amountPlayerDashes, amountPlayerDies;
    public float timeToCompleteGame;
    public int levelCurrentlyOn = 1, candyCollectedDuringRun = 0;

}
