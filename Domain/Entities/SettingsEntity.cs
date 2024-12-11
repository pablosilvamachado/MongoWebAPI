namespace WebAPI.Domain.Entities;

public record SettingsEntity : MongoEntity
{
    public int gridSize { get; set; } = 5;
    public int minBet { get; set; } = 1;
    public int maxBet { get; set; } = 700;
    public int min { get; set; } = 1;
    public int max { get; set; } = 20;
    public int maxPayout { get; set; } = 50000;

    public void ConfigInit(int gridSize,int minBet,int maxBet,int min,int max,int maxPayout)
    {
        this.gridSize = gridSize;
        this.minBet = minBet;
        this.maxBet = maxBet;
        this.min = min;
        this.max = max;
        this.maxPayout = maxPayout;
    }
}