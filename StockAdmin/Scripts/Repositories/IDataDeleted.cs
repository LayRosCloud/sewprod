using System.Threading.Tasks;

namespace StockAdmin.Scripts.Repositories;

public interface IDataDeleted
{
    Task DeleteAsync(int id);
}