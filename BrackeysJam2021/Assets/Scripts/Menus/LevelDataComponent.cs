using UnityEngine;

public class LevelDataComponent : MonoBehaviour
{
    public int indexLevel = 0;

    public void SaveLevel()
    {
        SaveSystem.SaveLevelData(this);
    }

    public void SaveLevel(int indexLevel)
    {
        this.indexLevel = indexLevel;
        SaveSystem.SaveLevelData(this);
    }

    public void LoadLevel()
    {
        LevelData data = SaveSystem.LoadData();

        indexLevel = data.previousIndexLevel;
    }
}
