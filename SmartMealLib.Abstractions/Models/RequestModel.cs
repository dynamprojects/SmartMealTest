namespace SmartMealLib.Abstractions.Models;

public class RequestModel
{
    public string Command { get; set; }
    public  System.Text.Json.JsonElement CommandParameters { get; set; }
}