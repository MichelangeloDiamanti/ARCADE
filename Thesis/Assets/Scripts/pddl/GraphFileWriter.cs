using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GraphFileWriter : MonoBehaviour
{
    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/example1.graphml";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        // GameData data = new GameData(currentScore, currentName, currentTimePlayed);
		string data = "";
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/example1.graphml";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        // GameData data = (GameData)bf.Deserialize(file);
        file.Close();
    }
}
