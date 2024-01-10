using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class PriceRepository: ICrud<PriceEntity>
{
    private const string EndPoint = "/v1/prices/";
    public async Task<List<PriceEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<PriceEntity>();
        List<PriceEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<PriceEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<PriceEntity>();
        PriceEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<PriceEntity> CreateAsync(PriceEntity entity)
    {
        var httpHandler = new HttpHandler<PriceEntity>();
        PriceEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<PriceEntity> UpdateAsync(PriceEntity entity)
    {
        var httpHandler = new HttpHandler<PriceEntity>();
        PriceEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<PriceEntity>();
        await httpHandler.DeleteAsync(EndPoint + id);
    }
}