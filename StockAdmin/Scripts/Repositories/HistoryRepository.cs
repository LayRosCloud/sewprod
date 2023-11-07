using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class HistoryRepository : IDataReader<History>
{
    private const string EndPoint = "/v1/histories/";
    public async Task<List<History>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<History>();
        List<History>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<History> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<History>();
        History? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }
}