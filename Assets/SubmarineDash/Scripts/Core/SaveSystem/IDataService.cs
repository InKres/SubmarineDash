
// Интерфейс для удобной замены сервиса сохранения в будующем
public interface IDataService
{
    void Save(GameData data);

    GameData Load();
}
