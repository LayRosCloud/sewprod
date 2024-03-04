using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class ModelOperationRepository : IModelOperationRepository
{
    private readonly HttpHandler<ModelOperationEntity> _handler = new();

    private const string EndPoint = "/v1/modeloperations/";
    
    public async Task<List<ModelOperationEntity>> GetAllAsync()
    {
        var response = await _handler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<ModelOperationEntity> GetAsync(int id)
    {
        var response = await _handler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<ModelOperationEntity> CreateAsync(ModelOperationEntity entity)
    {
        var response = await _handler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<ModelOperationEntity> UpdateAsync(ModelOperationEntity entity)
    {
        var response = await _handler.PutAsJsonAsync(EndPoint + entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(ModelOperationEntity item)
    {
        await _handler.DeleteAsync(EndPoint+item.Id);
    }
}