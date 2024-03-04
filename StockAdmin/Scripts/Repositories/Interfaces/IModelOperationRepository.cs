using System.Threading.Tasks;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IModelOperationRepository: IDataReader<ModelOperationEntity>, IDataCreator<ModelOperationEntity>, IDataPutter<ModelOperationEntity>
{
    Task DeleteAsync(ModelOperationEntity modelPrice);
}