using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbModelPriceRepository : ICrud<ModelPriceEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<ModelPriceEntity> CreateAsync(ModelPriceEntity entity)
    {
        var response = _db.modelPrices.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<ModelPriceEntity>> GetAllAsync()
    {
        var response = await _db.modelPrices.ToListAsync();
        return response;
    }

    public async Task<ModelPriceEntity> GetAsync(int id)
    {
        var response = await _db.modelPrices.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<ModelPriceEntity> UpdateAsync(ModelPriceEntity entity)
    {
        var response = _db.modelPrices.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.modelPrices.Remove(item);
        await _db.SaveChangesAsync();
    }
}