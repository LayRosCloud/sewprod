using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Repositories;

public class PersonRepository: IDataReader<Person>, 
    IDataCreator<Person>, 
    IDataPutter<Person>, 
    IDataDeleted
{
    private const string EndPoint = "/v1/persons/";
    
    public async Task<List<Person>> GetAllAsync()
    {
        var httpHandler = new HttpHandler<Person>();
        List<Person>? response = await httpHandler.GetListFromJsonAsync(EndPoint);
        return response!;
    }
    
    public async Task<Person> GetAsync(int id)
    {
        var httpHandler = new HttpHandler<Person>();
        Person? response = await httpHandler.GetFromJsonAsync(EndPoint+id);
        return response!;
    }
    
    public async Task<Auth?> LoginAsync(Person entity)
    {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync($"{ServerConstants.ServerAddress}/v1/auth", entity);

        Auth? auth = await responseMessage.Content.ReadFromJsonAsync<Auth>();
        return auth;
    }
    
    public async Task<Person> CreateAsync(Person entity)
    {
        var httpHandler = new HttpHandler<Person>();
        Person? response = await httpHandler.PostAsJsonAsync(EndPoint, entity);
        return response!;
    }

    public async Task<Person> UpdateAsync(Person entity)
    {
       var httpHandler = new HttpHandler<Person>();
        Person? response = await httpHandler.PutAsJsonAsync(EndPoint+entity.id, entity);
        return response!;
    }

    public async Task DeleteAsync(int id)
    {
        HttpHandler<Person> httpHandler = new HttpHandler<Person>();
        await httpHandler.DeleteAsync(EndPoint+id);
    }
}