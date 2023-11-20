using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PackageRepository : IDataReader<PackageEntity>, IDataCreator<PackageEntity>, IDataPutter<PackageEntity>, IDataDeleted
{
    private const string EndPoint = "/v1/packages/";
    public async Task<List<PackageEntity>?> GetAllAsync()
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();
        return await httpHandler.GetListFromJsonAsync(EndPoint);
    }
    
    public async Task<List<PackageEntity>?> GetAllAsync(int partyId)
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();
        return await httpHandler.GetListFromJsonAsync(EndPoint+$"?partyId={partyId}");
    }

    public async Task<PackageEntity?> GetAsync(int id)
    {
        HttpHandler<PackageEntity?> httpHandler = new HttpHandler<PackageEntity?>();
        return await httpHandler.GetFromJsonAsync(EndPoint+id);
    }

    public async Task<PackageEntity?> CreateAsync(PackageEntity entity)
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();
        return await httpHandler.PostAsJsonAsync(EndPoint, entity);
    }
    
    public async Task CreateAsync(List<PackageEntity> entities)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.Token.Token}");
        await client.PostAsJsonAsync($"{ServerConstants.ServerAddress}{EndPoint}range", entities);
    }

    public async Task<PackageEntity?> UpdateAsync(PackageEntity entity)
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();
        return await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
    }

    public async Task DeleteAsync(int id)
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}