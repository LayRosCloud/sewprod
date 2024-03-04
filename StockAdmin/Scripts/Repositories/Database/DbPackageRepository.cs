using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbPackageRepository : IPackagesRepository
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<PackageEntity> CreateAsync(PackageEntity entity)
    {
        var response = _db.packages.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<PackageEntity>> GetAllAsync()
    {
        var response = await _db.packages
            .Include(x => x.Material)
            .Include(x=> x.ClothOperations).ThenInclude(x=>x.Operation)
            .Include(x=>x.Person)
            .Include(x=>x.Size).ThenInclude(s=>s.Age)
            .ToListAsync();
        return response;
    }

    public async Task<PackageEntity> GetAsync(int id)
    {
        var response = await _db.packages
            .Include(x => x.Material)
            .Include(x=>x.ClothOperations).ThenInclude(x=>x.Operation)
            .Include(x=>x.Person)
            .Include(x=>x.Size).ThenInclude(s=>s.Age)
            .FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<PackageEntity> UpdateAsync(PackageEntity entity)
    {
        var response = _db.packages.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.packages.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<List<PackageEntity>> GetAllAsync(int personId)
    {
        var response = await _db.packages
            .Include(x => x.Material)
            .Include(x=>x.ClothOperations).ThenInclude(x=>x.Operation)
            .Include(x=>x.Person)
            .Include(x=>x.Size).ThenInclude(s=>s.Age)
            .Where(x=>x.PersonId==personId).ToListAsync();
        return response;
    }

    public async Task<List<PackageEntity>> GetAllOnPartyAsync(int partyId)
    {
        var response = await _db.packages
            .Include(x => x.Material)
            .Include(x=>x.ClothOperations).ThenInclude(x=>x.Operation)
            .Include(x=>x.Person)
            .Include(x=>x.Size).ThenInclude(s=>s.Age)
            .Where(x=>x.PartyId==partyId).ToListAsync();
        return response;
    }

    public async Task<List<PackageEntity>> CreateAsync(List<PackageEntity> list)
    {
        var response = new List<PackageEntity>();
        foreach (var item in list)
        {
            var createdItem = _db.packages.Add(item).Entity;
            response.Add(createdItem);
        }

        await _db.SaveChangesAsync();
        return response;
    }
}