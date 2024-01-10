using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IPerson: ICrud<PersonEntity>
{
    Task<AuthEntity?> LoginAsync(PersonEntity entity);
}