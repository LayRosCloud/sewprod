using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PostRepository: IDataReader<Post>, 
    IDataCreator<Post>, 
    IDataPutter<Post>, 
    IDataDeleted
{
    const string EndPoint = "/v1/posts/";

    public async Task<List<Post>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Post>();
        List<Post>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Post> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Post>();
        Post? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }

    public async Task<Post> CreateAsync(Post entity)
    {
        var httpHandler = new HttpHandler<Post>();
        Post? response = await httpHandler.GetFromJsonAsync(EndPoint);
        return response!;
    }

    public async Task<Post> UpdateAsync(Post entity)
    {
        var httpHandler = new HttpHandler<Post>();
        Post? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        var httpHandler = new HttpHandler<Post>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}