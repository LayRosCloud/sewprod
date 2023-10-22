using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockAdmin.Scripts.Repositories;

public interface IDataReader<TEntity>
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> GetAsync(int id);
}