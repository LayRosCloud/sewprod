using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbModelRepository : ICrud<ModelEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<ModelEntity> CreateAsync(ModelEntity entity)
    {
        var response = _db.models.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<ModelEntity>> GetAllAsync()
    {
        var response = await _db.models
            .Include(x=>x.Operations)
            .Include(x=>x.Prices)
            .Select(model => new ModelEntity()
            {
                Id = model.Id,
                CodeVendor = model.CodeVendor,
                Title = model.Title,
                Operations = model.Operations.Select(operation=> new OperationEntity()
                {
                    Id = operation.Id,
                    Percent = operation.Percent,
                    Description = operation.Description,
                    Name = operation.Name,
                    
                }).ToList(),
                Prices = model.Prices.Select(price => new PriceEntity()
                {
                    Id = price.Id,
                    Number = price.Number,
                    Date = price.Date,
                }).ToList()
            })
            .ToListAsync();
        return response;
    }

    public async Task<ModelEntity> GetAsync(int id)
    {
        var response = await _db.models
            .Include(x=>x.Operations)
            .Include(x=>x.Prices)
            .Select(model => new ModelEntity()
            {
                Id = model.Id,
                CodeVendor = model.CodeVendor,
                Title = model.Title,
                Operations = model.Operations.Select(operation=> new OperationEntity()
                {
                    Id = operation.Id,
                    Percent = operation.Percent,
                    Description = operation.Description,
                    Name = operation.Name,
                    ModelOperation = new ModelOperationEntity()
                    {
                        ModelId = model.Id,
                        OperationId = operation.Id
                    }
                }).ToList(),
                Prices = model.Prices.Select(price => new PriceEntity()
                {
                    Id = price.Id,
                    Number = price.Number,
                    Date = price.Date,
                    ModelPrice = new ModelPriceEntity()
                    {
                        PriceId = price.Id,
                        ModelId = model.Id
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<ModelEntity> UpdateAsync(ModelEntity entity)
    {
        var response = _db.models.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.models.Remove(item);
        await _db.SaveChangesAsync();
    }
}