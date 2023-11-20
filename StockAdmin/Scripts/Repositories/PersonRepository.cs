using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PersonRepository: IDataReader<PersonEntity>, 
    IDataCreator<PersonEntity>, 
    IDataPutter<PersonEntity>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/persons/";
    
    public async Task<List<PersonEntity>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<PersonEntity>();
        List<PersonEntity>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }
    
    public async Task<PersonEntity> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<PersonEntity>();
        PersonEntity? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }
    
    public async Task<AuthEntity?> LoginAsync(PersonEntity entity)
    {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync($"{ServerConstants.ServerAddress}/v1/auth", entity);

        AuthEntity? auth = await responseMessage.Content.ReadFromJsonAsync<AuthEntity>();
        return auth;
    }
    
    public async Task<PersonEntity> CreateAsync(PersonEntity entity)
    {
        var httpHandler = new HttpHandler<PersonEntity>();
        PersonEntity? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<PersonEntity> UpdateAsync(PersonEntity entity)
    {
       var httpHandler = new HttpHandler<PersonEntity>();
        PersonEntity? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.Id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        HttpHandler<PersonEntity> httpHandler = new HttpHandler<PersonEntity>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}