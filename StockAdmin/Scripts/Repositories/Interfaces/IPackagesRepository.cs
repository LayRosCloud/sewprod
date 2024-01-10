using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IPackagesRepository : ICrud<PackageEntity>
{
    Task<List<PackageEntity>> GetAllAsync(int personId);
    Task<List<PackageEntity>> GetAllOnPartyAsync(int partyId);
    Task<List<PackageEntity>> CreateAsync(List<PackageEntity> list);
}