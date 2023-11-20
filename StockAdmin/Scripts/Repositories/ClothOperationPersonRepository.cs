using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class ClothOperationPersonRepository : IDataReader<ClothOperationPersonEntity>, IDataCreator<ClothOperationPersonEntity>, IDataPutter<ClothOperationPersonEntity>, IDataDeleted
{
    private const string EndPoint = "/v1/clothoperationspersons/";

    public async Task<List<ClothOperationPersonEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<ClothOperationPersonEntity>();
        List<ClothOperationPersonEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<ClothOperationPersonEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperationPersonEntity>();
        ClothOperationPersonEntity? response = await httpHandler.GetFromJsonAsync(EndPoint + id);
        return response!;
    }

    public async Task<ClothOperationPersonEntity> CreateAsync(ClothOperationPersonEntity entity)
    {
        var httpHandler = new HttpHandler<ClothOperationPersonEntity>();
        ClothOperationPersonEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<ClothOperationPersonEntity> UpdateAsync(ClothOperationPersonEntity entity)
    {
        var httpHandler = new HttpHandler<ClothOperationPersonEntity>();
        ClothOperationPersonEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperationEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}