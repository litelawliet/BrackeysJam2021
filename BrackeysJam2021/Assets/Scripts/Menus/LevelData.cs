using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int previousIndexLevel;

    public LevelData(LevelDataComponent saveLevelData)
    {
        previousIndexLevel = saveLevelData.indexLevel;
    }
}
