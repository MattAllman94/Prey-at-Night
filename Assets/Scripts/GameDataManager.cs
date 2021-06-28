using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameSettingData
{
    public float bloodLevel;
    public CorruptionLevel corruptionLevel;
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
        //Load Game data
        data = LoadDataObject<GameDataObject>();

        if (INSTANCE != null)
        {
            Debug.Log("GameDataManager already instanced");
            return;
        }
        INSTANCE = this;

        //if data doesnt exist
        if(data == null)
        {
            //Initialize Game Data
            data = new GameDataObject();
            Debug.Log("NEW GAME DATA");

            //Initialize game setting dictionary
            data.gameData = new GameSettingData();

            //Initialize Player Data dictionary
            data.playerData = new PlayerData();

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
    public void SetGameData(GameManager _gamedata)
    {
        data.gameData.bloodLevel = _gamedata.currentBlood;
        data.gameData.corruptionLevel = _gamedata.corruptionLevel;
        data.gameData.powerPoints = _gamedata.powerPoints;
    }
    
    public void SetSettings(Settings _settings)
    {
        data.settingData.sounds = _settings.SFX;
        data.settingData.music = _settings.music;
        data.settingData.vfx = _settings.vfx;
    }

    public void SetPlayerData(PlayerController _player)
    {
        data.playerData.playerHealth = _player.currentHealth;
        data.playerData.lastPosition = _player.transform.position;
    }

    #endregion

    public GameManager GetGameManager()
    {
        return new GameManager(data.gameData.bloodLevel, data.gameData.corruptionLevel, data.gameData.powerPoints);
    }
    
    public Settings GetSettings()
    {
        return new Settings(data.settingData.sounds, data.settingData.music, data.settingData.vfx);
    }

    public override void SaveData()
    {
        //SaveTimePlayed();
        SaveDataObject(data);
    }

    public override void DeleteData()
    {
        DeleteDataObject();
    }
}
