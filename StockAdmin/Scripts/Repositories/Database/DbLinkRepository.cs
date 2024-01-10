using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbLinkRepository : IDataReader<LinkEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<List<LinkEntity>> GetAllAsync()
    {
        var response = await _db.links.ToListAsync();
        return response;
    }

    public async Task<LinkEntity> GetAsync(int id)
    {
        var response = await _db.links.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }
}