using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class PackageRepository : IPackagesRepository
{
    private const string EndPoint = "/v1/packages/";
    public async Task<List<PackageEntity>?> GetAllAsync()
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();
        return await httpHandler.GetListFromJsonAsync(EndPoint);
    }
    public async Task<List<PackageEntity>?> GetAllAsync(int personId)
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();

        return await httpHandler.GetListFromJsonAsync(EndPoint + $"?personId={personId}");
    }
    public async Task<List<PackageEntity>?> GetAllAsync(int month, int personId)
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();

        return await httpHandler.GetListFromJsonAsync(EndPoint + $"?month={month}&personId={personId}");
    }
    
    public async Task<List<PackageEntity>?> GetAllOnPartyAsync(int partyId)
    {
        HttpHandler<PackageEntity> httpHandler = new HttpHandler<PackageEntity>();

        return await httpHandler.GetListFromJsonAsync(EndPoint + $"?partyId={partyId}");
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
    
    public async Task<List<PackageEntity>?> CreateAsync(List<PackageEntity> entities)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ServerConstants.AuthorizationUser.Token}");
        var response = await client.PostAsJsonAsync($"{ServerConstants.ServerAddress}{EndPoint}range", entities);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var repository = new PersonRepository();
            AuthEntity? auth = await repository.LoginAsync(new PersonEntity() { Email = ServerConstants.Login, Password = ServerConstants.Password });
            ServerConstants.AuthorizationUser = auth!;
            response = await client.PostAsJsonAsync($"{ServerConstants.ServerAddress}{EndPoint}range", entities);
        }

        return await response.Content.ReadFromJsonAsync<List<PackageEntity>?>();
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