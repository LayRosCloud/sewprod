using System;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PermissionRepository : IDataCreator<PermissionEntity>
{
    private const String EndPoint = "/v1/permissions/";
    public async Task<PermissionEntity> CreateAsync(PermissionEntity entity)
    {
        var httpHandler = new HttpHandler<PermissionEntity>();
        PermissionEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }
}