using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IClothOperationPersonRepository: ICrud<ClothOperationPersonEntity>
{
    Task<List<ClothOperationPersonEntity>> GetAllAsync(int personId);
}