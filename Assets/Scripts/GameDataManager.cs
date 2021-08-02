using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameSettingData
{
    public float bloodLevel;
    public float corruptionLevel;
    public int powerPoints;
}

[Serializable]
public class SettingData //List what data we want to save
{
    public bool sounds;
    public bool music;
    public bool vfx;
}

[Serializable]
public class PlayerData
{
    public float playerHealth;
    public Vector3 lastPosition;
    public Quaternion lastRotation;
}

[Serializable]
public class PlayTimeData
{
    public int hoursPlayed = 0;
    public int minutesPlayed = 0;
    public int secondsPlayed = 0;
    public float totalSeconds = 0;
}

[Serializable]
public class GameDataObject //Put Data in here
{
    //Game Data Settings
    public GameSettingData gameData = new GameSettingData();

    //Game Settings
    public SettingData settingData = new SettingData();

    //Player Settings
    //public Dictionary<string, PlayerData> player = new Dictionary<string, PlayerData>();
    public PlayerData playerData = new PlayerData();

    //Times
    public PlayTimeData playTime = new PlayTimeData();

    public long dataObjectVersion = -1;
    public string dataObjectTimeStamp = null;
}

public class GameDataManager : GameData
{
    public GameDataObject data = new GameDataObject();

    public DateTime timeOfLastSave;

    public static GameDataManager INSTANCE;
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Debug.Log("GameDataManager already instanced");
            return;
        }
        INSTANCE = this;

        //Load Game data
        data = LoadDataObject<GameDataObject>();

        //if data doesnt exist
        if(data == null)
        {
            //Initialize Game Data
            data = new GameDataObject();
            Debug.Log("NEW GAME DATA");

            //Initialize game setting dictionary
            data.gameData = new GameSettingData();
            data.gameData.bloodLevel = 0;
            data.gameData.corruptionLevel = 0;
            data.gameData.powerPoints = 0;

            //Initialize Player Data dictionary
            data.playerData = new PlayerData();
            data.playerData.playerHealth = 100;
            data.playerData.lastPosition = new Vector3(-38, -3.8f, 63);
            data.playerData.lastRotation = new Quaternion(0, 180, 0, 1);

            //create time info if none
            data.playTime = new PlayTimeData();

            //Creat new setting and turn all to true
            data.settingData = new SettingData();
            data.settingData.sounds = true;
            data.settingData.music = true;
            data.settingData.vfx = true;
        }
        timeOfLastSave = DateTime.Now;
    }

    #region Setting Data 
    //Can all be combined when referencing singletons
    public void SetGameData(float _currentBlood, float _currentCorruption, int _powerPoints)
    {
        data.gameData.bloodLevel = _currentBlood;
        data.gameData.corruptionLevel = _currentCorruption;
        data.gameData.powerPoints = _powerPoints;
    }
    
    public void SetSettings(Settings _settings)
    {
        data.settingData.sounds = _settings.SFX;
        data.settingData.music = _settings.music;
        data.settingData.vfx = _settings.vfx;
    }

    public void SetPlayerData()
    {
        data.playerData.playerHealth = _P.currentHealth;
        data.playerData.lastPosition = _P.transform.position;
        data.playerData.lastRotation = _P.transform.rotation;
    }

    #endregion

    #region Retrieve Settings
    public float GetBloodLevel()
    {
        return data.gameData.bloodLevel;
    }

    public float GetCorruptionLevel()
    {
        return data.gameData.corruptionLevel;
    }

    public int GetPowerPoints()
    {
        return data.gameData.powerPoints;
    }

    public Settings GetSettings()
    {
        return new Settings(data.settingData.sounds, data.settingData.music, data.settingData.vfx);
    }

    public float GetCurrentHealth()
    {
        return data.playerData.playerHealth;
    }

    public Vector3 GetLastPosition()
    {
        return data.playerData.lastPosition;
    }

    public Quaternion GetLastRotation()
    {
        return data.playerData.lastRotation;
    }
    #endregion

    #region Functions
    public override void SaveData()
    {
        //SaveTimePlayed();
        SaveDataObject(data);
    }

    public override void DeleteData()
    {
        DeleteDataObject();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    #endregion
}
