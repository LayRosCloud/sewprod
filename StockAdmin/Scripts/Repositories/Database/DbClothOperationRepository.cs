using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbClothOperationRepository : IClothOperationRepository
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<ClothOperationEntity> CreateAsync(ClothOperationEntity entity)
    {
        var response = _db.clothOperations.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<ClothOperationEntity>> GetAllAsync()
    {
        var response = await _db.clothOperations
            .Include(x=> x.Operation)
            .Include(x=> x.Price)
            .Include(x=> x.ClothOperationPersons)
            .ToListAsync();
        return response;
    }

    public async Task<ClothOperationEntity> GetAsync(int id)
    {
        var response = await _db.clothOperations
            .Include(x=>x.Operation)
            .Include(x=>x.Price)
            .Include(x=>x.ClothOperationPersons)
            .FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<ClothOperationEntity> UpdateAsync(ClothOperationEntity entity)
    {
        var response = _db.clothOperations.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.clothOperations.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<List<ClothOperationEntity>> GetAllAsync(int packageId)
    {
        var response = await _db.clothOperations
            .Include(x=> x.Operation)
            .Include(x=> x.Price)
            .Include(x=> x.ClothOperationPersons)
            .Where(x=>x.PackageId == packageId).ToListAsync();
        return response;
    }
}