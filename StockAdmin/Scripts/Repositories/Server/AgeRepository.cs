using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class AgeRepository: ICrud<AgeEntity>
{
    private const string EndPoint = "/v1/ages/";

    public async Task<AgeEntity> CreateAsync(AgeEntity entity)
    {
        var httpHandler = new HttpHandler<AgeEntity>();
        AgeEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<List<AgeEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<AgeEntity>();
        List<AgeEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<AgeEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<AgeEntity>();
        AgeEntity? response = await httpHandler.GetFromJsonAsync($"{EndPoint}{id}");
        return response!;
    }

    public async Task<AgeEntity> UpdateAsync(AgeEntity entity)
    {
        var httpHandler = new HttpHandler<AgeEntity>();
        AgeEntity? response = await httpHandler.PutAsJsonAsync($"{EndPoint}{entity.Id}", entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<AgeEntity>();
        await httpHandler.DeleteAsync($"{EndPoint}{id}");
    }
}