// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

string filePath = "/Users/darkcite/Projects/ipfs-loader/ipfs-loader/123.png";
string fileUrl;

using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
{
    MemoryStream memoryStream = new MemoryStream();
    fileStream.CopyTo(memoryStream);

    // You can now work with the MemoryStream
    // If you want to read the MemoryStream as a string, for example:
    memoryStream.Position = 0; // Reset the position to the beginning of the stream
    using (StreamReader reader = new StreamReader(memoryStream))
    {
        var (apiKey, apiSecret) = GetInfuraCredentials();

        string apiUrl = "https://ipfs.infura.io:5001/api/v0/add?pin=true&cid-version=0&hash=sha2-256";
        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:{apiSecret}")));

        using var content = new MultipartFormDataContent();

        await fileStream.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        using var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png"); // MediaTypeHeaderValue.Parse(file.ContentType);
        content.Add(fileContent, "file", "123.png");

        var response = await httpClient.PostAsync(apiUrl, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<IPFSResponse>(responseJson);

        fileUrl = $"https://ipfs.infura.io/ipfs/{responseObject.Hash}";

        //var contractHandler = await new ContractHandlerProvider().GetContractHandlerAsync();
    }
}








static (string ApiKey, string ApiSecret) GetInfuraCredentials()
{
    // You can retrieve the API key and secret from a configuration file, environment variables, or other means.
    // In this example, I'm hardcoding them for simplicity, but you should use a more secure method.

    var apiKey = "";
    var apiSecret = "";

    return (apiKey, apiSecret);
}


public class IPFSResponse
{
    [JsonPropertyName("Hash")]
    public string Hash { get; set; }
}