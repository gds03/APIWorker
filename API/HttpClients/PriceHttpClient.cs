namespace API.HttpClients;

public interface IPriceHttpClient
{
    Task<decimal> GetPriceAsync(string sku);
}

public class PriceHttpClient : IPriceHttpClient
{
    public async Task<decimal> GetPriceAsync(string sku)
    {
        int v = new Random().Next(1, 2000);
        return Convert.ToDecimal(v);
    }
}