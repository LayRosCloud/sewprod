using System.Threading.Tasks;

namespace StockAdmin.Scripts.Repositories;

public interface IDataCreator<TEntity>
{
    Task<TEntity> CreateAsync(TEntity entity);
}