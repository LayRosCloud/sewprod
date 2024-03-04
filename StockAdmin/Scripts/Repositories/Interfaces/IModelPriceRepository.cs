using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IModelPriceRepository: IDataReader<ModelPriceEntity>, IDataCreator<ModelPriceEntity>, IDataPutter<ModelPriceEntity>
{
    Task DeleteAsync(ModelPriceEntity modelPrice);
}