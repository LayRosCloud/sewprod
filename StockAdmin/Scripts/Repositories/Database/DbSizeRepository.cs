using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbSizeRepository : ISizesRepository
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<SizeEntity> CreateAsync(SizeEntity entity)
    {
        var response = _db.sizes.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<SizeEntity>> GetAllAsync()
    {
        var response = await _db.sizes.ToListAsync();
        return response;
    }

    public async Task<SizeEntity> GetAsync(int id)
    {
        var response = await _db.sizes.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<SizeEntity> UpdateAsync(SizeEntity entity)
    {
        var response = _db.sizes.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.sizes.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<List<SizeEntity>> GetAllAsync(int ageId)
    {
        var response = await _db.sizes.Where(x=>x.AgeId == ageId).ToListAsync();
        return response;
    }
}