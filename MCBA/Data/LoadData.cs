using Newtonsoft.Json;
using MCBA.Model.DTO;
namespace MCBA.Connection.Request;

public class LoadData
{
	private const string Url =
		"https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";


	public LoadData()
	{
		RestApiRequest(Url);
	}

	private void RestApiRequest(string url)
	{
		using var client = new HttpClient();
		var json = client.GetStringAsync(url).Result;

		// Debug remove later
		Console.WriteLine(json);

		//var customer = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
		//{
		//	DateFormatString = "dd/MM/yyyy hh:mm:ss tt" 
		//}) ;
	}
}