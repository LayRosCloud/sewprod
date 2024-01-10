using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class SizeRepository: ISizesRepository
{
    public Task<List<SizeEntity>> GetAllAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<SizeEntity> GetAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<SizeEntity> CreateAsync(SizeEntity entity)
    {
        throw new System.NotImplementedException();
    }

    public Task<SizeEntity> UpdateAsync(SizeEntity entity)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<SizeEntity>> GetAllAsync(int ageId)
    {
        throw new System.NotImplementedException();
    }
}