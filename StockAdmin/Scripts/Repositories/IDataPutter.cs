using System.Threading.Tasks;

namespace StockAdmin.Scripts.Repositories;

public interface IDataPutter<TEntity>
{
    Task<TEntity> UpdateAsync(TEntity entity);
}