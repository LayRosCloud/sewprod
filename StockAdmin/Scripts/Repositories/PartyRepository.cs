using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PartyRepository: IDataReader<Party>, 
    IDataCreator<Party>, 
    IDataPutter<Party>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/parties/";

    public async Task<List<Party>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Party>();
        List<Party>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Party> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Party>();
        Party? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<Party> CreateAsync(Party entity)
    {
        var httpHandler = new HttpHandler<Party>();
        Party? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<Party> UpdateAsync(Party entity)
    {
        var httpHandler = new HttpHandler<Party>();
        Party? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Party>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}