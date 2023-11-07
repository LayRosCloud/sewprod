using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class ColorRepository : IDataReader<Color>, IDataCreator<Color>, IDataDeleted, IDataPutter<Color>
{
    private const string EndPoint = "/v1/colors/";

    public async Task<Color> CreateAsync(Color entity)
    {
        var httpHandler = new HttpHandler<Color>();
        Color? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<List<Color>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Color>();
        List<Color>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Color> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Color>();
        Color? response = await httpHandler.GetFromJsonAsync($"{EndPoint}{id}");
        return response!;
    }

    public async Task<Color> UpdateAsync(Color entity)
    {
        var httpHandler = new HttpHandler<Color>();
        Color? response = await httpHandler.PutAsJsonAsync($"{EndPoint}{entity.id}", entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Color>();
        await httpHandler.DeleteAsync($"{EndPoint}{id}");
    }
}