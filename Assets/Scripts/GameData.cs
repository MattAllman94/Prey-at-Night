using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class GameData : Prey
{
    //Date format we want to use
    public static string dateFormat = "yyyy-MM-dd HH:mm:ss zzz";
    //Change this to whatever you want
    protected string fileName = "data.pat";
    //Subdirectory for the save file
    string subDir = "Save";

    public abstract void SaveData(); 

    public abstract void DeleteData();

    //void OnApplicationQuit()
    //{
    //    SaveData();
    //}

    //private void OnApplicationFocus(bool appInFocus)
    //{
    //    if (!appInFocus)
    //    {
    //        SaveData();
    //    }
    //}

    protected string MakeTimeStampNow()
    {
        return DateTime.Now.ToString(dateFormat);
    }

    protected T LoadDataObject<T>() where T : GameDataObject
    {
        //ensure the file exists
        if(File.Exists(GetPath()))
        {

            //Create filestream for opening files
            FileStream stream = new FileStream(GetPath(), FileMode.Open);

            //Create a StreamReader
            StreamReader reader = new StreamReader(stream);

            //Read the entire file into a String value
            string jSave = reader.ReadToEnd();

            //Close the Stream
            stream.Close();

            //Returmn tje decrypted string converted to json then as a GameDataObject Type
            return JsonUtility.FromJson<T>(jSave);
            
        }
        else
        {
            Debug.Log("Save File was not found in the path");
            return null;
        }
    }

    protected void SaveDataObject<T>(T obj) where T : GameDataObject
    {
        //Increment our save version
        obj.dataObjectVersion++;

        //Put a timestamp onto the object
        obj.dataObjectTimeStamp = MakeTimeStampNow();

        //Create the save directory if it does not exist
        Directory.CreateDirectory(Path.GetDirectoryName(GetPath()));

        //Convert the GameDataObject to json then return as string
        string jSave = JsonUtility.ToJson(obj);

        //Create Filestream for opening files
        FileStream stream = new FileStream(GetPath(), FileMode.Create);

        //Create StreamWriter
        StreamWriter writer = new StreamWriter(stream);

        //Write to the innermost stream
        writer.Write(jSave);

        //Close Stream Writer
        writer.Close();

        stream.Close();

        Debug.Log("SaveDataObject");
    }

    protected void DeleteDataObject()
    {
        if(File.Exists(GetPath()))
        {
            Debug.Log("Deleting file " + fileName);
            File.Delete(GetPath());
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        else
        {
            Debug.Log("No File found in " + GetPath());
        }

        //Reload the game from 0
#if UNITY_EDITOR
        SceneManager.LoadScene(0);
#endif
    }

    string GetPath()
    {
        return Application.persistentDataPath + "/" + subDir + "/" + fileName;
        //return Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/" + subDir + "/" + fileName;
    }
}
