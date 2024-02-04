using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class SizeRepository: ISizesRepository
{
    private const string EndPoint = "/v1/sizes/";
    public async Task<List<SizeEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<SizeEntity>();
        List<SizeEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<SizeEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<SizeEntity>();
        SizeEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<SizeEntity> CreateAsync(SizeEntity entity)
    {
        var httpHandler = new HttpHandler<SizeEntity>();
        SizeEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<SizeEntity> UpdateAsync(SizeEntity entity)
    {
        var httpHandler = new HttpHandler<SizeEntity>();
        SizeEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<PriceEntity>();
        await httpHandler.DeleteAsync(EndPoint + id);
    }

    public async Task<List<SizeEntity>> GetAllAsync(int ageId)
    {
        var httpHandler = new HttpHandler<SizeEntity>();
        List<SizeEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint+"?ageId="+ageId);
        return response!;
    }
}