using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class OperationRepository: IDataReader<Operation>, IDataCreator<Operation>, IDataPutter<Operation>, IDataDeleted
{
    private const string EndPoint = "/v1/operations/";


    public async Task<List<Operation>> GetAllAsync()
    {
        HttpHandler<Operation> httpHandler = new HttpHandler<Operation>();
        List<Operation>? list = await httpHandler.GetListFromJsonAsync(EndPoint);
        return list!;
    }

    public async Task<Operation> GetAsync(int id)
    {
        HttpHandler<Operation> httpHandler = new HttpHandler<Operation>();
        Operation? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<Operation> CreateAsync(Operation entity)
    {
        HttpHandler<Operation> httpHandler = new HttpHandler<Operation>();
        Operation? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<Operation> UpdateAsync(Operation entity)
    {
        HttpHandler<Operation> httpHandler = new HttpHandler<Operation>();
        Operation? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        HttpHandler<Operation> httpHandler = new HttpHandler<Operation>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}