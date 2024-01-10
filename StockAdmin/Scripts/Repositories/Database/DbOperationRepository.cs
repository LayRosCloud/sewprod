using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbOperationRepository : ICrud<OperationEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<OperationEntity> CreateAsync(OperationEntity entity)
    {
        var response = _db.operations.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<OperationEntity>> GetAllAsync()
    {
        var response = await _db.operations.ToListAsync();
        return response;
    }

    public async Task<OperationEntity> GetAsync(int id)
    {
        var response = await _db.operations.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<OperationEntity> UpdateAsync(OperationEntity entity)
    {
        var response = _db.operations.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.operations.Remove(item);
        await _db.SaveChangesAsync();
    }
}