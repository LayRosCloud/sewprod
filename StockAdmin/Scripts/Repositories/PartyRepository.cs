using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PartyRepository: IDataReader<PartyEntity>, 
    IDataCreator<PartyEntity>, 
    IDataPutter<PartyEntity>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/parties/";

    public async Task<List<PartyEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<PartyEntity>();
        List<PartyEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<PartyEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<PartyEntity>();
        PartyEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<PartyEntity> CreateAsync(PartyEntity entity)
    {
        var httpHandler = new HttpHandler<PartyEntity>();
        PartyEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<PartyEntity> UpdateAsync(PartyEntity entity)
    {
        var httpHandler = new HttpHandler<PartyEntity>();
        PartyEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<PartyEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}