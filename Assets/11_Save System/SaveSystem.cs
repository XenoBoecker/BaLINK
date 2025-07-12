using UnityEngine;

public static class SaveSystem
{
    private static SaveData _loadedSaveData;
    private static SaveFileManager<SaveData> _saveFileManager;
    
    public static SaveData Data => _loadedSaveData;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        _saveFileManager = new SaveFileManager<SaveData>("SaveData");
        LoadSaveData();
    }

    public static void LoadSaveData()
    {
        _loadedSaveData = _saveFileManager.LoadDataClass(); 
    }

    public static void SaveSaveData()
    {
        _saveFileManager.SaveDataClass(_loadedSaveData);
    }
}