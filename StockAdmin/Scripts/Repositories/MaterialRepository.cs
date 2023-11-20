using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class MaterialRepository : IDataReader<MaterialEntity>, IDataCreator<MaterialEntity>, IDataDeleted, IDataPutter<MaterialEntity>
{
    private const string EndPoint = "/v1/materials/";

    public async Task<MaterialEntity> CreateAsync(MaterialEntity entity)
    {
        var httpHandler = new HttpHandler<MaterialEntity>();
        MaterialEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<List<MaterialEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<MaterialEntity>();
        List<MaterialEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<MaterialEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<MaterialEntity>();
        MaterialEntity? response = await httpHandler.GetFromJsonAsync($"{EndPoint}{id}");
        return response!;
    }

    public async Task<MaterialEntity> UpdateAsync(MaterialEntity entity)
    {
        var httpHandler = new HttpHandler<MaterialEntity>();
        MaterialEntity? response = await httpHandler.PutAsJsonAsync($"{EndPoint}{entity.Id}", entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<MaterialEntity>();
        await httpHandler.DeleteAsync($"{EndPoint}{id}");
    }
}