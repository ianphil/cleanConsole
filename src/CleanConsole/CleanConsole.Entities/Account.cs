namespace CleanConsole.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<WeatherForecast> WeatherForcasts { get; set; }
    public bool Enabled { get; set; }

    public Account()
    {
        WeatherForcasts = new HashSet<WeatherForecast>();
    }
}