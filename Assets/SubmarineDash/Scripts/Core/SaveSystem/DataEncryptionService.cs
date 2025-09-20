using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataEncryptionService
{
    private readonly byte[] encryptionKey = { 0x15, 0x2F, 0x7A, 0xE9, 0x3C, 0x91, 0xB4, 0x58 };

    private string persistentDataPath;

    public void Init()
    {
        persistentDataPath = Application.persistentDataPath;
    }

    public async void SaveData(IPersistentData data, string fileName, Action onSuccess = null, Action<string> onError = null)
    {
        try
        {
            string path = GetFilePath(fileName);

            await Task.Run(() =>
            {
                string jsonData = data.ToJson();
                byte[] encryptedData = EncryptString(jsonData);
                File.WriteAllBytes(path, encryptedData);
            });

            Debug.Log($"Data saved to: {path}");
            onSuccess?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
            onError?.Invoke(e.Message);
        }
    }

    public async void LoadData<T>(string fileName, Action<T> onSuccess = null, Action<string> onError = null) where T : IPersistentData, new()
    {
        try
        {
            string path = GetFilePath(fileName);

            T result = await Task.Run(() =>
            {

                if (!File.Exists(path))
                    return default(T);

                byte[] encryptedData = File.ReadAllBytes(path);
                string jsonData = DecryptBytes(encryptedData);

                T data = new T();
                data.FromJson(jsonData);
                return data;
            });

            Debug.Log($"Data loaded from: {path}");
            onSuccess?.Invoke(result);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading data from \"{fileName}\": {e.Message}");
            onError?.Invoke(e.Message);
        }
    }

    public async void DeleteFile(string fileName, Action onSuccess = null, Action<string> onError = null)
    {
        try
        {
            await Task.Run(() =>
            {
                string path = GetFilePath(fileName);

                if (File.Exists(path))
                    File.Delete(path);
            });

            Debug.Log($"File deleted: {fileName}");
            onSuccess?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting file \"{fileName}\": {e.Message}");
            onError?.Invoke(e.Message);
        }
    }

    public bool FileExists(string fileName)
    {
        return File.Exists(GetFilePath(fileName));
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(persistentDataPath, fileName);
    }

    private byte[] EncryptString(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        return PerformEncryption(bytes);
    }

    private string DecryptBytes(byte[] data)
    {
        byte[] decryptedBytes = PerformEncryption(data);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private byte[] PerformEncryption(byte[] data)
    {
        byte[] result = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            result[i] = (byte)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }
        return result;
    }
}