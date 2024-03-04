using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbPartyRepository : IPartiesRepository
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<PartyEntity> CreateAsync(PartyEntity entity)
    {
        var response = _db.parties.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<PartyEntity>> GetAllAsync()
    {
        var response = await _db.parties
            .Include(x=>x.Model)
            .Include(x=>x.Price)
            .Select(
                p => new PartyEntity()
                {
                    Id = p.Id,
                    CutNumber = p.CutNumber,
                    PriceId = p.PriceId,
                    PersonId = p.PersonId,
                    DateStart = p.DateStart,
                    DateEnd = p.DateEnd,
                    ModelId = p.ModelId,
                    Price = new PriceEntity()
                    {
                        Id = p.Price.Id,
                        Number = p.Price.Number,
                        Date = p.Price.Date
                    },
                    Person = p.Person,
                    Model = new ModelEntity()
                    {
                        Id = p.Model.Id,
                        Title = p.Model.Title,
                        CodeVendor = p.Model.CodeVendor
                    }
                }
                
            )
            .ToListAsync();
        return response;
    }

    public async Task<PartyEntity> GetAsync(int id)
    {
        var response = await _db.parties
            .Include(x=>x.Model)
            .Include(x=>x.Price)
            .Select(
                p => new PartyEntity()
                {
                    Id = p.Id,
                    CutNumber = p.CutNumber,
                    PriceId = p.PriceId,
                    PersonId = p.PersonId,
                    DateStart = p.DateStart,
                    DateEnd = p.DateEnd,
                    ModelId = p.ModelId,
                    Price = new PriceEntity()
                    {
                        Id = p.Price.Id,
                        Number = p.Price.Number,
                        Date = p.Price.Date
                    },
                    Person = p.Person,
                    Model = new ModelEntity()
                    {
                        Id = p.Model.Id,
                        Title = p.Model.Title,
                        CodeVendor = p.Model.CodeVendor
                    }
                }
                
            )
            .FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<PartyEntity> UpdateAsync(PartyEntity entity)
    {
        var response = _db.parties.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.parties.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<List<PartyEntity>> GetAllAsync(int personId)
    {
        var response = await _db.parties
            .Include(x=>x.Model)
            .Include(x=>x.Price)
            .Select(
                p => new PartyEntity()
                {
                    Id = p.Id,
                    CutNumber = p.CutNumber,
                    PriceId = p.PriceId,
                    PersonId = p.PersonId,
                    DateStart = p.DateStart,
                    DateEnd = p.DateEnd,
                    ModelId = p.ModelId,
                    Price = new PriceEntity()
                    {
                        Id = p.Price.Id,
                        Number = p.Price.Number,
                        Date = p.Price.Date
                    },
                    Person = p.Person,
                    Model = new ModelEntity()
                    {
                        Id = p.Model.Id,
                        Title = p.Model.Title,
                        CodeVendor = p.Model.CodeVendor
                    }
                }
                
            )
            .Where(x=>x.PersonId == personId).ToListAsync();
        return response;
    }
}