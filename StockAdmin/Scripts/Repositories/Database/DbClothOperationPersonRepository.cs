using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbClothOperationPersonRepository : IClothOperationPersonRepository
{
    private readonly DataContext _db = DataContext.Instance;

    public async Task<ClothOperationPersonEntity> CreateAsync(ClothOperationPersonEntity entity)
    {
        var response = _db.clothOperationsPersons.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<ClothOperationPersonEntity>> GetAllAsync()
    {
        var response = await _db.clothOperationsPersons
            .Include(x=>x.Person)
            .Include(x=>x.ClothOperation).ThenInclude(t=>t.Price)
            .ToListAsync();
        return response;
    }

    public async Task<ClothOperationPersonEntity> GetAsync(int id)
    {
        var response = await _db.clothOperationsPersons
            .Include(x=>x.Person)
            .Include(x=>x.ClothOperation).ThenInclude(t=>t.Price)
            .FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<ClothOperationPersonEntity> UpdateAsync(ClothOperationPersonEntity entity)
    {
        var response = _db.clothOperationsPersons.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.clothOperationsPersons.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<List<ClothOperationPersonEntity>> GetAllAsync(int personId)
    {
        var response = await _db.clothOperationsPersons
            .Include(x=>x.ClothOperation).ThenInclude(t=>t.Price)
            .Where(x=>x.PersonId == personId)
            .ToListAsync();
        return response;
    }
}