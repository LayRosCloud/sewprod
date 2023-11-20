using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class ModelRepository: IDataReader<ModelEntity>, 
    IDataCreator<ModelEntity>, 
    IDataPutter<ModelEntity>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/models/";

    public async Task<List<ModelEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<ModelEntity>();
        List<ModelEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<ModelEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<ModelEntity>();
        ModelEntity? response = await httpHandler.GetFromJsonAsync(EndPoint + id);
        return response!;
    }

    public async Task<ModelEntity> CreateAsync(ModelEntity entity)
    {
        var httpHandler = new HttpHandler<ModelEntity>();
        ModelEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<ModelEntity> UpdateAsync(ModelEntity entity)
    {
        var httpHandler = new HttpHandler<ModelEntity>();
        ModelEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<ModelEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}