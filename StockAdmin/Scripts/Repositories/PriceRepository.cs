using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PriceRepository: IDataReader<Price>, 
    IDataCreator<Price>, 
    IDataPutter<Price>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/prices/";
    public async Task<List<Price>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Price>();
        List<Price>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Price> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Price>();
        Price? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<Price> CreateAsync(Price entity)
    {
        var httpHandler = new HttpHandler<Price>();
        Price? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<Price> UpdateAsync(Price entity)
    {
        var httpHandler = new HttpHandler<Price>();
        Price? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Price>();
        await httpHandler.DeleteAsync(EndPoint + id);
    }
}