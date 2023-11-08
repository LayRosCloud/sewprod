using System;
using System.Net;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PermissionRepository : IDataCreator<Permission>
{
    private const String EndPoint = "/v1/permissions/";
    public async Task<Permission> CreateAsync(Permission entity)
    {
        var httpHandler = new HttpHandler<Permission>();
        Permission? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }
}