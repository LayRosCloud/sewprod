using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbHistoryRepository : IDataReader<HistoryEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<List<HistoryEntity>> GetAllAsync()
    {
        var response = await _db.histories.ToListAsync();
        return response;
    }

    public async Task<HistoryEntity> GetAsync(int id)
    {
        var response = await _db.histories.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }
}