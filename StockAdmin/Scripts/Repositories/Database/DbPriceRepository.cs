using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbPriceRepository : ICrud<PriceEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<PriceEntity> CreateAsync(PriceEntity entity)
    {
        var response = _db.prices.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<PriceEntity>> GetAllAsync()
    {
        var response = await _db.prices.ToListAsync();
        return response;
    }

    public async Task<PriceEntity> GetAsync(int id)
    {
        var response = await _db.prices.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<PriceEntity> UpdateAsync(PriceEntity entity)
    {
        var response = _db.prices.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.prices.Remove(item);
        await _db.SaveChangesAsync();
    }
}