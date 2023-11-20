using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class HistoryRepository : IDataReader<HistoryEntity>
{
    private const string EndPoint = "/v1/histories/";
    public async Task<List<HistoryEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<HistoryEntity>();
        List<HistoryEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<HistoryEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<HistoryEntity>();
        HistoryEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }
}