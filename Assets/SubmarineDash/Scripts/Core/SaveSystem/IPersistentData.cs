public interface IPersistentData
{
    string ToJson();
    void FromJson(string json);
}
