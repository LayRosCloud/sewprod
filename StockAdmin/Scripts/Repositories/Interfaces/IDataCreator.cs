using System.Threading.Tasks;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IDataCreator<TEntity>
{
    Task<TEntity> CreateAsync(TEntity entity);
}