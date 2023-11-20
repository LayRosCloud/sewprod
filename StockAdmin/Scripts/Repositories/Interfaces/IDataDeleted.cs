using System.Threading.Tasks;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IDataDeleted
{
    Task DeleteAsync(int id);
}