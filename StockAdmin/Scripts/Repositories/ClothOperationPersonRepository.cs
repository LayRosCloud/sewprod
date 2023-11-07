using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class ClothOperationPersonRepository : IDataReader<ClothOperationPerson>, IDataCreator<ClothOperationPerson>, IDataPutter<ClothOperationPerson>, IDataDeleted
{
    private const string EndPoint = "/v1/clothoperationspersons/";

    public async Task<List<ClothOperationPerson>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<ClothOperationPerson>();
        List<ClothOperationPerson>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<ClothOperationPerson> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperationPerson>();
        ClothOperationPerson? response = await httpHandler.GetFromJsonAsync(EndPoint + id);
        return response!;
    }

    public async Task<ClothOperationPerson> CreateAsync(ClothOperationPerson entity)
    {
        var httpHandler = new HttpHandler<ClothOperationPerson>();
        ClothOperationPerson? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<ClothOperationPerson> UpdateAsync(ClothOperationPerson entity)
    {
        var httpHandler = new HttpHandler<ClothOperationPerson>();
        ClothOperationPerson? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperation>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}