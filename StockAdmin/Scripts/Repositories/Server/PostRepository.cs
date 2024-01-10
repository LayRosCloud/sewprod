using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories.Server;

public class PostRepository: ICrud<PostEntity>
{
    const string EndPoint = "/v1/posts/";

    public async Task<List<PostEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<PostEntity>();
        List<PostEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<PostEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<PostEntity>();
        PostEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<PostEntity> CreateAsync(PostEntity entity)
    {
        var httpHandler = new HttpHandler<PostEntity>();
        PostEntity? response = await httpHandler.GetFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<PostEntity> UpdateAsync(PostEntity entity)
    {
        var httpHandler = new HttpHandler<PostEntity>();
        PostEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<PostEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}