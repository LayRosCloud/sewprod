using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Scripts.Server;

public class HttpHandler<TEntity>
{
    public async Task<List<TEntity>?> GetListFromJsonAsync(string point)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
        var responseMessage = await httpClient.GetAsync($"{ServerConstants.ServerAddress}{point}");
        if (responseMessage.IsSuccessStatusCode == false)
        {
            await RefreshToken(responseMessage);
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
            responseMessage = await httpClient.GetAsync($"{ServerConstants.ServerAddress}{point}");
        }
        return await responseMessage.Content.ReadFromJsonAsync<List<TEntity>>();
    }
    
    public async Task<TEntity?> GetFromJsonAsync(string point)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");

        var responseMessage = await httpClient.GetAsync($"{ServerConstants.ServerAddress}{point}");
        if (responseMessage.IsSuccessStatusCode == false)
        {
            await RefreshToken(responseMessage);
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
            responseMessage = await httpClient.GetAsync($"{ServerConstants.ServerAddress}{point}");
        }
        return await responseMessage.Content.ReadFromJsonAsync<TEntity>();
    }
    
    public async Task<TEntity?> PostAsJsonAsync(string point, TEntity entity)
    {
        HttpClient httpClient = new HttpClient();
        
        if (ServerConstants.Token != null)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
        }


        HttpResponseMessage responseMessage =
            await httpClient.PostAsJsonAsync($"{ServerConstants.ServerAddress}{point}", entity);
        if (responseMessage.IsSuccessStatusCode == false)
        {
            await RefreshToken(responseMessage);
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
            responseMessage = 
                await httpClient.PostAsJsonAsync($"{ServerConstants.ServerAddress}{point}", entity);
            
        }
        return await responseMessage.Content.ReadFromJsonAsync<TEntity>();
    }

    public async Task<TEntity?> PutAsJsonAsync(string point, TEntity entity)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");

        var responseMessage = await httpClient.PutAsJsonAsync($"{ServerConstants.ServerAddress}{point}", entity);
        
        if (responseMessage.IsSuccessStatusCode == false)
        {
            await RefreshToken(responseMessage);
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
            responseMessage =
                await httpClient.PutAsJsonAsync($"{ServerConstants.ServerAddress}{point}", entity);
        }

        return await responseMessage.Content.ReadFromJsonAsync<TEntity>();
    }
    
    public async Task DeleteAsync(string point)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");

        var responseMessage = await httpClient.DeleteAsync($"{ServerConstants.ServerAddress}{point}");
        if (responseMessage.IsSuccessStatusCode == false)
        {
            await RefreshToken(responseMessage);
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
            await httpClient.DeleteAsync($"{ServerConstants.ServerAddress}{point}");
        }


    }

    private async Task RefreshToken(HttpResponseMessage responseMessage)
    {
        if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            var repository = new PersonRepository();
            AuthEntity? auth = await repository.LoginAsync(new PersonEntity() { Email = ServerConstants.Login, Password = ServerConstants.Password });
            ServerConstants.Token = auth!;
        }
    }
}