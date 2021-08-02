using UnityEngine;
using UnityEditor;

public class DeletePlayerData
{
    [MenuItem("Game Data/Delete All Game Data")]
    private static void DeleteAllLevelData()
    {
        if (EditorUtility.DisplayDialog("Delete Game Data", "Are you sure you want to delete all Game Data?", "Yes", "No"))
        {
            //GameData.DeleteAllGameData();
            GameDataManager.INSTANCE.DeleteData();
            Debug.LogWarning("All Data Deleted");
        }
        
    }
}