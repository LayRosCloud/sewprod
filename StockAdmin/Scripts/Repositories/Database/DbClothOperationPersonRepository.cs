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
        var response = _db.clothOperationPersons.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<ClothOperationPersonEntity>> GetAllAsync()
    {
        var response = await _db.clothOperationPersons.ToListAsync();
        return response;
    }

    public async Task<ClothOperationPersonEntity> GetAsync(int id)
    {
        var response = await _db.clothOperationPersons.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<ClothOperationPersonEntity> UpdateAsync(ClothOperationPersonEntity entity)
    {
        var response = _db.clothOperationPersons.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.clothOperationPersons.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<List<ClothOperationPersonEntity>> GetAllAsync(int personId)
    {
        var response = await _db.clothOperationPersons.Where(x=>x.PersonId == personId).ToListAsync();
        return response;
    }
}