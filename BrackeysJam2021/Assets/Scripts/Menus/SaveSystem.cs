using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveLevelData(LevelDataComponent levelData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levelIndex.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(levelData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadData() 
    {
        string path = Application.persistentDataPath + "/levelIndex.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}

