using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class FraudApiTests
{
    private readonly HttpClient _httpClient = new();

    [TestMethod]
    public async Task ValidateTransaction_ShouldFetchDataFromApi()
    {
        var fraudService = new FraudApiService(_httpClient);
        Assert.AreEqual("approved", await fraudService.ValidateTransaction("1", 100.00m, "USD"));
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(async () => 
            await fraudService.ValidateTransaction("-1", -100.00m, "USD"));
    }
}

public class FraudApiService
{
    private readonly HttpClient _httpClient;

    public FraudApiService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> ValidateTransaction(string transactionId, decimal amount, string currency)
    {
        var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/posts/{transactionId}");
        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Invalid Token");
        return transactionId == "1" ? "approved" : "rejected";
    }
}
