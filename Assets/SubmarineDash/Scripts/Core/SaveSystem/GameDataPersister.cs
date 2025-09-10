using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GameDataPersister : MonoBehaviour, IDataService
{
    // ���� ��� �������� ����������
    private readonly byte[] encryptionKey = { 0x15, 0x2F, 0x7A, 0xE9, 0x3C, 0x91, 0xB4, 0x58 };

    public void Save(GameData gameData)
    {
        try
        {
            // ��������� ���� � ������� Path.Combine
            string path = Path.Combine(Application.persistentDataPath, "SaveFile.json");

            // ������������ ������ � JSON
            string jsonData = JsonUtility.ToJson(gameData, true);

            // ������� JSON ������ � �����
            byte[] encryptedData = EncryptString(jsonData);

            // ��������� ������������� ����� � ����
            File.WriteAllBytes(path, encryptedData);

            Debug.Log($"Game saved successfully to: {path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game data: {e.Message}");
        }
    }

    public GameData Load()
    {
        try
        {
            // ��������� ���� � ������� Path.Combine
            string path = Path.Combine(Application.persistentDataPath, "SaveFile.save");

            if (File.Exists(path))
            {
                // ������ ������������� ����� �� �����
                byte[] encryptedData = File.ReadAllBytes(path);

                // ��������� ����� ������� � JSON ������
                string jsonData = DecryptBytes(encryptedData);

                // ������������ JSON ������� � ������ GameData
                GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

                Debug.Log($"Game loaded successfully from: {path}");
                return gameData;
            }
            else
            {
                Debug.LogWarning($"Save file not found in: {path}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game data: {e.Message}");
            return null;
        }
    }

    // �������������� ����� ��� �������� ������������� ����� ����������
    public bool SaveFileExists()
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveFile.save");
        return File.Exists(path);
    }

    // �������������� ����� ��� �������� ����� ����������
    public void DeleteSaveFile()
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, "SaveFile.save");
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Save file deleted successfully");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting save file: {e.Message}");
        }
    }

    // ���������� ������ � �����
    private byte[] EncryptString(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        return PerformEncryption(bytes);
    }

    // ������������ ������ � ������
    private string DecryptBytes(byte[] data)
    {
        byte[] decryptedBytes = PerformEncryption(data);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    // �������� ����� XOR-����������/������������
    private byte[] PerformEncryption(byte[] data)
    {
        byte[] result = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            // XOR ���������� � ������ (����������� ������������� �����)
            result[i] = (byte)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }

        return result;
    }
}