using UnityEngine;

public abstract class BasePersistence<T> : MonoBehaviour where T : IPersistentData, new()
{
    [Header("Persistence Settings")]
    [SerializeField] protected string fileName = "default.save";

    protected DataEncryptionService encryptionService;
    protected T currentData;

    public virtual void Init()
    {
        encryptionService = new DataEncryptionService();
        currentData = new T();
    }

    public virtual void SaveData(T data)
    {
        if (data == null)
        {
            Debug.LogError("Cannot save null data");
            return;
        }

        currentData = data;
        encryptionService.SaveData(data, fileName);
        Debug.Log($"Data saved to {fileName}");
    }

    public virtual T LoadData()
    {
        currentData = encryptionService.LoadData<T>(fileName);

        if (currentData == null)
        {
            currentData = new T();
            Debug.Log($"Created new default data for {fileName}");
        }
        else
        {
            Debug.Log($"Data loaded from {fileName}");
        }

        return currentData;
    }

    public virtual bool FileExists()
    {
        return encryptionService.FileExists(fileName);
    }

    public virtual void DeleteFile()
    {
        encryptionService.DeleteFile(fileName);
        currentData = new T();
        Debug.Log($"File {fileName} deleted and data reset");
    }

    public virtual T GetCurrentData()
    {
        return currentData;
    }
}