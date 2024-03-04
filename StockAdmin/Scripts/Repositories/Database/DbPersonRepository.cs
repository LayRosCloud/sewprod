using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;
using StockAdmin.Scripts.Exceptions;

namespace StockAdmin.Scripts.Repositories.Database;

public class DbPersonRepository : IPerson
{
    private readonly DataContext _db = DataContext.Instance;
    
    public async Task<PersonEntity> CreateAsync(PersonEntity entity)
    {
        var response = _db.persons.Add(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task<List<PersonEntity>> GetAllAsync()
    {
        var response = await _db.persons.Include(x => x.Posts).ToListAsync();
        return response;
    }

    public async Task<PersonEntity> GetAsync(int id)
    {
        var response = await _db.persons
            .Include(x => x.Posts)
            .FirstOrDefaultAsync(x=>x.Id == id);
        return response;
    }

    public async Task<PersonEntity> UpdateAsync(PersonEntity entity)
    {
        var response = _db.persons.Update(entity).Entity;
        await _db.SaveChangesAsync();
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _db.persons.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<AuthEntity?> LoginAsync(PersonEntity entity)
    {
        var item = await _db.persons
            .Include(x => x.Posts)
            .FirstOrDefaultAsync(x => x.Email == entity.Email);
        if (item == null)
        {
            throw new AuthException();
        }

        string hash = HashPassword(entity.Password);
        bool isVerified = VerifyPassword(item.Password, hash);
        if (!isVerified)
        {
            throw new AuthException();
        }

        return new AuthEntity()
        {
            Email = item.Email,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Token = hash
        };
    }

    private static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private static bool VerifyPassword(string hashedPasswordFromDatabase, string hashedPassword)
    {
        return hashedPassword.Equals(hashedPasswordFromDatabase, StringComparison.OrdinalIgnoreCase);
    }
}