using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class OperationRepository: IDataReader<OperationEntity>, IDataCreator<OperationEntity>, IDataPutter<OperationEntity>, IDataDeleted
{
    private const string EndPoint = "/v1/operations/";


    public async Task<List<OperationEntity>> GetAllAsync()
    {
        HttpHandler<OperationEntity> httpHandler = new HttpHandler<OperationEntity>();
        List<OperationEntity>? list = await httpHandler.GetListFromJsonAsync(EndPoint);
        return list!;
    }

    public async Task<OperationEntity> GetAsync(int id)
    {
        HttpHandler<OperationEntity> httpHandler = new HttpHandler<OperationEntity>();
        OperationEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<OperationEntity> CreateAsync(OperationEntity entity)
    {
        HttpHandler<OperationEntity> httpHandler = new HttpHandler<OperationEntity>();
        OperationEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<OperationEntity> UpdateAsync(OperationEntity entity)
    {
        HttpHandler<OperationEntity> httpHandler = new HttpHandler<OperationEntity>();
        OperationEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        HttpHandler<OperationEntity> httpHandler = new HttpHandler<OperationEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}