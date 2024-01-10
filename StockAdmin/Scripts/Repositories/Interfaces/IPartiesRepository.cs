using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IPartiesRepository : ICrud<PartyEntity>
{
    Task<List<PartyEntity>> GetAllAsync(int personId);
}