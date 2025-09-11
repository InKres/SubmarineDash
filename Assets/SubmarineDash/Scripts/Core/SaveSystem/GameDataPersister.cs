using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GameDataPersister : MonoBehaviour, IDataService
{
    // Ключ для простого шифрования
    private readonly byte[] encryptionKey = { 0x15, 0x2F, 0x7A, 0xE9, 0x3C, 0x91, 0xB4, 0x58 };

    // Путь до файла сохранения
    private readonly string path = Path.Combine(Application.persistentDataPath, "SaveFile.json");

    public GameData CurrentSavedData { get; private set; }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Save(GameData gameData)
    {
        try
        {
            // Конвертируем данные в JSON
            string jsonData = JsonUtility.ToJson(gameData, true);

            // Шифруем JSON строку в байты
            byte[] encryptedData = EncryptString(jsonData);

            // Сохраняем зашифрованные байты в файл
            File.WriteAllBytes(path, encryptedData);

            CurrentSavedData = new GameData(gameData);

            Debug.Log($"Game saved successfully to: {path}", this);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game data: {e.Message}", this);
        }
    }

    public GameData Load()
    {
        try
        {
            if (SaveFileExists())
            {
                // Читаем зашифрованные байты из файла
                byte[] encryptedData = File.ReadAllBytes(path);

                // Дешифруем байты обратно в JSON строку
                string jsonData = DecryptBytes(encryptedData);

                // Конвертируем JSON обратно в объект GameData
                GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

                CurrentSavedData = new GameData(gameData);

                Debug.Log($"Game loaded successfully from: {path}", this);
                return gameData;
            }
            else
            {
                Debug.LogWarning($"Save file not found in: {path}", this);
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game data: {e.Message}", this);
            return null;
        }
    }

    // Дополнительный метод для проверки существования файла сохранения
    public bool SaveFileExists()
    {
        return File.Exists(path);
    }

    // Дополнительный метод для удаления файла сохранения
    public void DeleteSaveFile()
    {
        try
        {
            if (SaveFileExists())
            {
                File.Delete(path);
                Debug.Log("Save file deleted successfully", this);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting save file: {e.Message}", this);
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

public class GameDataPersisterAdapter : MonoBehaviour
{
    [Header("Model")]
    [SerializeField]
    private GameDataPersister persister;

    [Header("Persistent Components")]
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField]
    private DifficultyController difficultyController;

    public static GameDataPersisterAdapter instance { get; private set; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        persister = FindAnyObjectByType<GameDataPersister>();

        if (persister == null)
        {
            Debug.LogError($"GameDataPersister component not found!!!", this);
        }
    }

    public void SaveSessionData()
    {
        int currentScore = scoreController.Score;
        float currentDifficulty = difficultyController.CurrentDifficultyValue;

        GameData gameData = new GameData(0, 0, 0);
        gameData.currentScore = currentScore;
        gameData.currentDifficulty = currentDifficulty;

        if (persister.CurrentSavedData != null)
        {
            gameData.recordScore = persister.CurrentSavedData.recordScore;
        }

        persister.Save(gameData);
    }

    public void SaveOnlyRecordScore()
    {
        int currentScore = scoreController.Score;

        GameData gameData = new GameData(0, 0, 0);
        SaveRecordScore(gameData, currentScore);

        persister.Save(gameData);
    }

    public GameData Load()
    {
        if (persister.SaveFileExists())
        {
            return persister.Load();
        }

        return null;
    }

    private GameData SaveRecordScore(GameData gameData, int currentScore)
    {
        if (persister.CurrentSavedData != null)
        {
            gameData.recordScore = persister.CurrentSavedData.recordScore;

            if (gameData.recordScore < currentScore)
            {
                gameData.recordScore = currentScore;
            }
        }
        else
        {
            gameData.recordScore = currentScore;
        }

        return gameData;
    }
}