using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class SizeRepository: IDataReader<Size>, 
    IDataCreator<Size>, 
    IDataPutter<Size>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/sizes/";
    
    public async Task<List<Size>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Size>();
        List<Size>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Size> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Size>();
        Size? response = await httpHandler.GetFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Size> CreateAsync(Size entity)
    {
        var httpHandler = new HttpHandler<Size>();
        Size? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<Size> UpdateAsync(Size entity)
    {
        var httpHandler = new HttpHandler<Size>();
        Size? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Size>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}