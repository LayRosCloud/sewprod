using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbModelOperationRepository : IModelOperationRepository
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<ModelOperationEntity> CreateAsync(ModelOperationEntity entity)
    {
        var response = _db.modelOperations.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<ModelOperationEntity>> GetAllAsync()
    {
        var response = await _db.modelOperations.ToListAsync();
        return response;
    }

    public async Task<ModelOperationEntity> GetAsync(int id)
    {
        var response = await _db.modelOperations.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<ModelOperationEntity> UpdateAsync(ModelOperationEntity entity)
    {
        var response = _db.modelOperations.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(ModelOperationEntity modelOperation)
    {
        var item = await _db.modelOperations
            .FirstOrDefaultAsync(x => x.ModelId == modelOperation.ModelId
                                      && modelOperation.OperationId == x.OperationId);
        _db.modelOperations.Remove(item);
        await _db.SaveChangesAsync();
    }
}