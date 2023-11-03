using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PackageRepository : IDataReader<Package>, IDataCreator<Package>, IDataPutter<Package>, IDataDeleted
{
    private const string EndPoint = "/v1/packages/";
    public async Task<List<Package>?> GetAllAsync()
    {
        HttpHandler<Package> httpHandler = new HttpHandler<Package>();
        return await httpHandler.GetListFromJsonAsync(EndPoint);
    }

    public async Task<Package?> GetAsync(int id)
    {
        HttpHandler<Package?> httpHandler = new HttpHandler<Package?>();
        return await httpHandler.GetFromJsonAsync(EndPoint+id);
    }

    public async Task<Package?> CreateAsync(Package entity)
    {
        HttpHandler<Package> httpHandler = new HttpHandler<Package>();
        return await httpHandler.PostAsJsonAsync(EndPoint, entity);
    }
    
    public async Task CreateAsync(List<Package> entities)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token}");
        await client.PostAsJsonAsync($"{ServerConstants.ServerAddress}{EndPoint}range", entities);
    }

    public async Task<Package?> UpdateAsync(Package entity)
    {
        HttpHandler<Package> httpHandler = new HttpHandler<Package>();
        return await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
    }

    public async Task DeleteAsync(int id)
    {
        HttpHandler<Package> httpHandler = new HttpHandler<Package>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}