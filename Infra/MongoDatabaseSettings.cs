namespace WebAPI.Infra;

public class MongoDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string SettingsCollectionName { get; set; } = null!;
    public string UserCollectionName { get; set; }
}