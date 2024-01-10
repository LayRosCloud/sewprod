using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IClothOperationRepository : ICrud<ClothOperationEntity>
{
    Task<List<ClothOperationEntity>> GetAllAsync(int packageId);
}