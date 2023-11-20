using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class ClothOperationRepository: IDataReader<ClothOperationEntity>, 
                                        IDataCreator<ClothOperationEntity>, 
                                        IDataPutter<ClothOperationEntity>, 
                                        IDataDeleted
{
    private const string EndPoint = "/v1/clothoperations/";

    public async Task<List<ClothOperationEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<ClothOperationEntity>();
        List<ClothOperationEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }
    
    public async Task<List<ClothOperationEntity>> GetAllAsync(int packageId)
    {
        var httpHandler = new HttpHandler<ClothOperationEntity>();
        List<ClothOperationEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint+$"?packageId={packageId}");
        return response!;
    }

    public async Task<ClothOperationEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperationEntity>();
        ClothOperationEntity? response = await httpHandler.GetFromJsonAsync(EndPoint + id);
        return response!;
    }

    public async Task<ClothOperationEntity> CreateAsync(ClothOperationEntity entity)
    {
        var httpHandler = new HttpHandler<ClothOperationEntity>();
        ClothOperationEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<ClothOperationEntity> UpdateAsync(ClothOperationEntity entity)
    {
        var httpHandler = new HttpHandler<ClothOperationEntity>();
        ClothOperationEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperationEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}