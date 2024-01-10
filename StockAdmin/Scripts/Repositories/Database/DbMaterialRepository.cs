using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbMaterialRepository : ICrud<MaterialEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<MaterialEntity> CreateAsync(MaterialEntity entity)
    {
        var response = _db.materials.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<MaterialEntity>> GetAllAsync()
    {
        var response = await _db.materials.ToListAsync();
        return response;
    }

    public async Task<MaterialEntity> GetAsync(int id)
    {
        var response = await _db.materials.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<MaterialEntity> UpdateAsync(MaterialEntity entity)
    {
        var response = _db.materials.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.materials.Remove(item);
        await _db.SaveChangesAsync();
    }
}