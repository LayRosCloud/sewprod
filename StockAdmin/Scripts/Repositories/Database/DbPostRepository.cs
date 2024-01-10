using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbPostRepository : ICrud<PostEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<PostEntity> CreateAsync(PostEntity entity)
    {
        var response = _db.posts.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<PostEntity>> GetAllAsync()
    {
        var response = await _db.posts.ToListAsync();
        return response;
    }

    public async Task<PostEntity> GetAsync(int id)
    {
        var response = await _db.posts.FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<PostEntity> UpdateAsync(PostEntity entity)
    {
        var response = _db.posts.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.posts.Remove(item);
        await _db.SaveChangesAsync();
    }
}