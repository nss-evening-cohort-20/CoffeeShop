using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace CoffeeShop.Repositories;

public class CoffeeRepository : BaseRepository, ICoffeeRepository
{
    private const string _coffeeSql = @"SELECT c.Id
                                              ,c.Title
                                              ,c.BeanVarietyId
                                              ,b.[Name]
                                              ,b.Region
                                              ,b.Notes
                                        FROM Coffee c
                                        JOIN BeanVariety b on b.Id = c.BeanVarietyId ";

    public CoffeeRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public List<Coffee> GetAll()
    {
        using var conn = Connection;
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = _coffeeSql;
        
        using var reader = cmd.ExecuteReader();
        List<Coffee> results = new();
        
        while (reader.Read())
        {
            results.Add(CoffeeFromReader(reader));
        }

        return results;
    }

    
    public Coffee? GetById(int id)
    {
        using var conn = Connection;
        conn.Open();
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"{_coffeeSql} WHERE c.Id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        
        using var reader = cmd.ExecuteReader();
        Coffee? result = null;
        
        if (reader.Read())
        {
            result = CoffeeFromReader(reader);
        }
        
        return result;
    }

    private Coffee CoffeeFromReader(SqlDataReader reader)
    {
        return new Coffee()
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Title = reader.GetString(reader.GetOrdinal("Title")),
            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
            BeanVariety = new()
            {
                Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Region = reader.GetString(reader.GetOrdinal("Region")),
                Notes = !reader.IsDBNull(reader.GetOrdinal("Notes"))
                        ? reader.GetString(reader.GetOrdinal("Notes"))
                        : ""
            }
        };
    }
}
