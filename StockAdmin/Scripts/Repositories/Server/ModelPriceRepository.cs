using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class ModelPriceRepository : IModelPriceRepository
{
    private readonly HttpHandler<ModelPriceEntity> _handler = new();

    private const string EndPoint = "/v1/modelprices/";
    
    public async Task<List<ModelPriceEntity>> GetAllAsync()
    {
        var response = await _handler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<ModelPriceEntity> GetAsync(int id)
    {
        var response = await _handler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<ModelPriceEntity> CreateAsync(ModelPriceEntity entity)
    {
        var response = await _handler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<ModelPriceEntity> UpdateAsync(ModelPriceEntity entity)
    {
        var response = await _handler.PutAsJsonAsync(EndPoint + entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(ModelPriceEntity item)
    {
        await _handler.DeleteAsync(EndPoint + item.Id);
    }
}