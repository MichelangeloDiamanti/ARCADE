using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Xml;
using System.Xml.Serialization;

public class FileWriter : MonoBehaviour
{
    public void SaveFile(string data)
    {
        string destination = InitialWorld.path + "/example1.graphml";
        System.IO.File.WriteAllText (destination, data);
        Debug.Log(InitialWorld.path);
        // FileStream file;

        // if (File.Exists(destination)) file = File.OpenWrite(destination);
        // else file = File.Create(destination);

        // // GameData data = new GameData(currentScore, currentName, currentTimePlayed);
        // BinaryFormatter bf = new BinaryFormatter();
        // bf.Serialize(file, data);
        // file.Close();
    }

    public void SaveFile(string filename, string data)
    {
        string destination = InitialWorld.path + "/log/" + filename;
        System.IO.File.WriteAllText (destination, data);
        Debug.Log(InitialWorld.path);
    }

    public void LoadFile()
    {
        string destination = InitialWorld.path + "/example1.graphml";
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

    public static string GenerateLogName(){
        string result = "log-" + DateTime.Now +".txt";
        result = result.Replace("/", "");
        result = result.Replace(":", "");
        result = result.Replace(" ", "");
        return result;
    }

    /// <summary>
    /// Serializes an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializableObject"></param>
    /// <param name="fileName"></param>
    public static void SerializeObject<T>(T serializableObject, string fileName)
    {
        if (serializableObject == null) { return; }

        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, serializableObject);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(InitialWorld.path + "/graph/" +fileName);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.StackTrace);
        }
    }


    /// <summary>
    /// Deserializes an xml file into an object list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T DeSerializeObject<T>(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) { return default(T); }

        T objectOut = default(T);

        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(InitialWorld.path + "/graph/" +fileName);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(T);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    objectOut = (T)serializer.Deserialize(reader);
                }
            }
        }
        catch (Exception ex)
        {
            //Log exception here
        }

        return objectOut;
    }
}
