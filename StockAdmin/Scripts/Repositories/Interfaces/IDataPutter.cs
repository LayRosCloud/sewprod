using System.Threading.Tasks;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IDataPutter<TEntity>
{
    Task<TEntity> UpdateAsync(TEntity entity);
}