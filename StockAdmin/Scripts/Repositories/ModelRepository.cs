using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class ModelRepository: IDataReader<Model>, 
    IDataCreator<Model>, 
    IDataPutter<Model>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/models/";

    public async Task<List<Model>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Model>();
        List<Model>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Model> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Model>();
        Model? response = await httpHandler.GetFromJsonAsync(EndPoint + id);
        return response!;
    }

    public async Task<Model> CreateAsync(Model entity)
    {
        var httpHandler = new HttpHandler<Model>();
        Model? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<Model> UpdateAsync(Model entity)
    {
        var httpHandler = new HttpHandler<Model>();
        Model? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Model>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}