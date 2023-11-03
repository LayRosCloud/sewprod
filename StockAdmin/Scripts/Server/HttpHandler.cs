using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace StockAdmin.Scripts.Server;

public class HttpHandler<TEntity>
{
    public async Task<List<TEntity>?> GetListFromJsonAsync(string point)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.token}");
        return await httpClient.GetFromJsonAsync<List<TEntity>>($"{ServerConstants.ServerAddress}{point}");
    }
    public async Task<TEntity?> GetFromJsonAsync(string point)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.token}");
        return await httpClient.GetFromJsonAsync<TEntity>($"{ServerConstants.ServerAddress}{point}");
    }
    
    public async Task<TEntity?> PostAsJsonAsync(string point, TEntity entity)
    {
        HttpClient httpClient = new HttpClient();
        if (ServerConstants.Token != null)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.token}");
        }
        HttpResponseMessage responseMessage =
            await httpClient.PostAsJsonAsync($"{ServerConstants.ServerAddress}{point}", entity);
        return await responseMessage.Content.ReadFromJsonAsync<TEntity>();
    }

    public async Task<TEntity?> PutAsJsonAsync(string point, TEntity entity)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.token}");
        HttpResponseMessage responseMessage =
            await httpClient.PutAsJsonAsync($"{ServerConstants.ServerAddress}{point}", entity);
        return await responseMessage.Content.ReadFromJsonAsync<TEntity>();
    }
    
    public async Task DeleteAsync(string point)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.token}");
        await httpClient.DeleteAsync($"{ServerConstants.ServerAddress}{point}");
    }
}