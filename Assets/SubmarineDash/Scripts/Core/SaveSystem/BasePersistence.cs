using System;
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

    public virtual void SaveData(T data, Action onSuccess = null, Action<string> onError = null)
    {
        if (data == null)
        {
            onError?.Invoke("Cannot save null data");
            return;
        }

        currentData = data;
        encryptionService.SaveData(data, fileName, onSuccess, onError);
    }

    public virtual void LoadData(Action<T> onSuccess = null, Action<string> onError = null)

    {
        encryptionService.LoadData<T>(fileName,
            data =>
            {
                currentData = data;
                onSuccess?.Invoke(data);
            },
            onError);
    }

    public virtual void DeleteFile(Action onSuccess = null, Action<string> onError = null)
    {
        encryptionService.DeleteFile(fileName, onSuccess, onError);
    }

    public virtual bool FileExists()
    {
        return encryptionService.FileExists(fileName);
    }

    public virtual T GetCurrentData()
    {
        return currentData;
    }
}