using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GameDataPersister : MonoBehaviour, IDataService
{
    // Ключ для простого шифрования
    private readonly byte[] encryptionKey = { 0x15, 0x2F, 0x7A, 0xE9, 0x3C, 0x91, 0xB4, 0x58 };

    public void Save(GameData gameData)
    {
        try
        {
            // Формируем путь с помощью Path.Combine
            string path = Path.Combine(Application.persistentDataPath, "SaveFile.json");

            // Конвертируем данные в JSON
            string jsonData = JsonUtility.ToJson(gameData, true);

            // Шифруем JSON строку в байты
            byte[] encryptedData = EncryptString(jsonData);

            // Сохраняем зашифрованные байты в файл
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
            // Формируем путь с помощью Path.Combine
            string path = Path.Combine(Application.persistentDataPath, "SaveFile.save");

            if (File.Exists(path))
            {
                // Читаем зашифрованные байты из файла
                byte[] encryptedData = File.ReadAllBytes(path);

                // Дешифруем байты обратно в JSON строку
                string jsonData = DecryptBytes(encryptedData);

                // Конвертируем JSON обратно в объект GameData
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

    // Дополнительный метод для проверки существования файла сохранения
    public bool SaveFileExists()
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveFile.save");
        return File.Exists(path);
    }

    // Дополнительный метод для удаления файла сохранения
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

    // Шифрование строки в байты
    private byte[] EncryptString(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        return PerformEncryption(bytes);
    }

    // Дешифрование байтов в строку
    private string DecryptBytes(byte[] data)
    {
        byte[] decryptedBytes = PerformEncryption(data);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    // Основной метод XOR-шифрования/дешифрования
    private byte[] PerformEncryption(byte[] data)
    {
        byte[] result = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            // XOR шифрование с ключом (циклическое использование ключа)
            result[i] = (byte)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }

        return result;
    }
}