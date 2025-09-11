using System;
using System.IO;
using System.Text;
using UnityEngine;

public class DataEncryptionService
{
    private readonly byte[] encryptionKey = { 0x15, 0x2F, 0x7A, 0xE9, 0x3C, 0x91, 0xB4, 0x58 };

    public void SaveData(IPersistentData data, string fileName)
    {
        try
        {
            string path = GetFilePath(fileName);
            string jsonData = data.ToJson();
            byte[] encryptedData = EncryptString(jsonData);
            File.WriteAllBytes(path, encryptedData);
            Debug.Log($"Data saved to: {path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }

    public T LoadData<T>(string fileName) where T : IPersistentData, new()
    {
        try
        {
            string path = GetFilePath(fileName);

            if (File.Exists(path))
            {
                byte[] encryptedData = File.ReadAllBytes(path);
                string jsonData = DecryptBytes(encryptedData);

                T data = new T();
                data.FromJson(jsonData);

                Debug.Log($"Data loaded from: {path}");
                return data;
            }

            Debug.LogWarning($"File not found: {path}");
            return new T();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading data: {e.Message}");
            return new T();
        }
    }

    public bool FileExists(string fileName)
    {
        return File.Exists(GetFilePath(fileName));
    }

    public void DeleteFile(string fileName)
    {
        try
        {
            string path = GetFilePath(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"File deleted: {path}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting file: {e.Message}");
        }
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
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