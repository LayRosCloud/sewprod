using StockAdmin.Models;

namespace StockAdmin.Scripts.Server;

public class ServerConstants
{
    public const string ServerAddress = "http://localhost:5000";
    public static Auth Token = new();
    public static string Login = "";
    public static string Password = "";
}