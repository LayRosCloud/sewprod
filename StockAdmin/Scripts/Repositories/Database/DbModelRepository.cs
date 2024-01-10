﻿using System.Collections.Generic;
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
        var response = await _db.models.ToListAsync();
        return response;
    }

    public async Task<ModelEntity> GetAsync(int id)
    {
        var response = await _db.models.FirstOrDefaultAsync(x=>x.Id == id);
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