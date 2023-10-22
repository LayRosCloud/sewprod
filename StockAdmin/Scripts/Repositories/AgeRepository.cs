using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class AgeRepository : IDataReader<Age>, 
    IDataCreator<Age>, 
    IDataPutter<Age>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/ages/";

    public async Task<Age> CreateAsync(Age entity)
    {
        var httpHandler = new HttpHandler<Age>();
        Age? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<List<Age>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Age>();
        List<Age>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Age> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Age>();
        Age? response = await httpHandler.GetFromJsonAsync($"{EndPoint}{id}");
        return response!;
    }

    public async Task<Age> UpdateAsync(Age entity)
    {
        var httpHandler = new HttpHandler<Age>();
        Age? response = await httpHandler.PutAsJsonAsync($"{EndPoint}{entity.id}", entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Age>();
        await httpHandler.DeleteAsync($"{EndPoint}{id}");
    }
}