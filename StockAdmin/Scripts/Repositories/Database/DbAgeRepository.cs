using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbAgeRepository: ICrud<AgeEntity>
{
    private readonly DataContext _db = DataContext.Instance;

    public async Task<AgeEntity> CreateAsync(AgeEntity entity)
    {
        var response = _db.ages.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<AgeEntity>> GetAllAsync()
    {
        var response = await _db.ages.ToListAsync();
        return response;
    }

    public async Task<AgeEntity> GetAsync(int id)
    {
        var response = await _db.ages.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<AgeEntity> UpdateAsync(AgeEntity entity)
    {
        var response = _db.ages.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.ages.Remove(item);
        await _db.SaveChangesAsync();
    }
}