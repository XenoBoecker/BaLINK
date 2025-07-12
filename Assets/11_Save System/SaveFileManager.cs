using System;
using System.IO;
using UnityEngine;

public class SaveFileManager<T> where T : class
{
    public SaveFileManager(string fileName)
    {
        _path = Application.persistentDataPath + $"/{fileName}.json";
    }

    private string _path;

    public T LoadDataClass()
    {
        T loadedData = null;

        if (TryLoadDataAtPath(_path, out string loadedJson))
        {
            loadedData = JsonUtility.FromJson<T>(loadedJson);
        }
        else
        {
            //if the data file dosen't exist then create a new one and save it
            T newDataClass = Activator.CreateInstance<T>();
            SaveDataClass(newDataClass);
        }

        return loadedData;
    }

    public void SaveDataClass(T data)
    {
        SaveDataToPath(_path, data);
    }

    private void SaveDataToPath(string path, T data)
    {
        string jsonData = JsonUtility.ToJson(data);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(jsonData);
            }
        }
    }

    public static bool TryLoadDataAtPath(string path, out string loadedData)
    {
        loadedData = "";
        if (!File.Exists(path))
        {
            return false;
        }

        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                loadedData = reader.ReadToEnd();
            }
        }

        return true;
    }
}
