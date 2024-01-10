using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbPermissionRepository : IDataCreator<PermissionEntity>
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<PermissionEntity> CreateAsync(PermissionEntity entity)
    {
        var response = _db.permissions.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }
}