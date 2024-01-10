using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface ICrud<TEntity>: IDataReader<TEntity>, IDataCreator<TEntity>, IDataPutter<TEntity>, IDataDeleted
{
    
}