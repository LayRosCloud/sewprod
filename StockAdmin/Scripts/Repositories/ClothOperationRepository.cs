using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class ClothOperationRepository: IDataReader<ClothOperation>, 
                                        IDataCreator<ClothOperation>, 
                                        IDataPutter<ClothOperation>, 
                                        IDataDeleted
{
    private const string EndPoint = "/v1/clothoperations/";

    public async Task<List<ClothOperation>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<ClothOperation>();
        List<ClothOperation>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }
    
    public async Task<List<ClothOperation>> GetAllAsync(int partyId)
    {
        var httpHandler = new HttpHandler<ClothOperation>();
        List<ClothOperation>? response = await httpHandler.GetListFromJsonAsync(EndPoint+$"?partyId={partyId}");
        return response!;
    }

    public async Task<ClothOperation> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperation>();
        ClothOperation? response = await httpHandler.GetFromJsonAsync(EndPoint + id);
        return response!;
    }

    public async Task<ClothOperation> CreateAsync(ClothOperation entity)
    {
        var httpHandler = new HttpHandler<ClothOperation>();
        ClothOperation? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<ClothOperation> UpdateAsync(ClothOperation entity)
    {
        var httpHandler = new HttpHandler<ClothOperation>();
        ClothOperation? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<ClothOperation>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}