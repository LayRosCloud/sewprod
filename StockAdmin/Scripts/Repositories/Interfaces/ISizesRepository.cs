using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface ISizesRepository : ICrud<SizeEntity>
{
    Task<List<SizeEntity>> GetAllAsync(int ageId);
}