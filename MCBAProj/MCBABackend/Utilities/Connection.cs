using Microsoft.Extensions.Configuration;
namespace MCBAConnection;

public static class Connection
{
    // Return's Databse Connection
	public static String? GetConnection()
	{
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        return configuration.GetConnectionString(nameof(MCBAConnection));
    }
}