using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class LinkRepository : IDataReader<Link>
{
    private const string EndPoint = "/";
    
    public async Task<List<Link>> GetAllAsync()
    {
        HttpHandler<Link> httpHandler = new HttpHandler<Link>();
        List<Link>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Link> GetAsync(int id)
    {
        HttpHandler<Link> httpHandler = new HttpHandler<Link>();
        Link? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }
}