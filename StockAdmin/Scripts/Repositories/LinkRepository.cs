using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class LinkRepository : IDataReader<LinkEntity>
{
    private const string EndPoint = "/";
    
    public async Task<List<LinkEntity>> GetAllAsync()
    {
        HttpHandler<LinkEntity> httpHandler = new HttpHandler<LinkEntity>();
        List<LinkEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<LinkEntity> GetAsync(int id)
    {
        HttpHandler<LinkEntity> httpHandler = new HttpHandler<LinkEntity>();
        LinkEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }
}