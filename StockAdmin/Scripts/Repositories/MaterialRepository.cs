using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class MaterialRepository : IDataReader<Material>, IDataCreator<Material>, IDataDeleted, IDataPutter<Material>
{
    private const string EndPoint = "/v1/materials/";

    public async Task<Material> CreateAsync(Material entity)
    {
        var httpHandler = new HttpHandler<Material>();
        Material? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<List<Material>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Material>();
        List<Material>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Material> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Material>();
        Material? response = await httpHandler.GetFromJsonAsync($"{EndPoint}{id}");
        return response!;
    }

    public async Task<Material> UpdateAsync(Material entity)
    {
        var httpHandler = new HttpHandler<Material>();
        Material? response = await httpHandler.PutAsJsonAsync($"{EndPoint}{entity.id}", entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Material>();
        await httpHandler.DeleteAsync($"{EndPoint}{id}");
    }
}