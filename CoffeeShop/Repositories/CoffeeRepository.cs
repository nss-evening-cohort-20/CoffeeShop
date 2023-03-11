using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace CoffeeShop.Repositories;

public class CoffeeRepository : BaseRepository, ICoffeeRepository
{
    private const string _coffeeSelect = @"SELECT c.Id
                                                 ,c.Title
                                                 ,c.BeanVarietyId
                                                 ,b.[Name]
                                                 ,b.Region
                                                 ,b.Notes
                                           FROM Coffee c
                                           JOIN BeanVariety b on b.Id = c.BeanVarietyId ";

    private const string _coffeeInsert = @"INSERT INTO Coffee
                                               (Title, BeanVarietyId)
                                           OUTPUT INSERTED.Id
                                           VALUES
                                               (@title, @beanVarietyId)";

    private const string _coffeeUpdate = @"UPDATE Coffee
                                           SET Title = @title,
                                               BeanVarietyId = @beanVarietyId
                                           WHERE Id = @id";

    private const string _coffeeDelete = @"DELETE FROM Coffee
                                           WHERE Id = @id";

    public CoffeeRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public List<Coffee> GetAll()
    {
        using var conn = Connection;
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = _coffeeSelect;

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
        cmd.CommandText = $"{_coffeeSelect} WHERE c.Id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        
        using var reader = cmd.ExecuteReader();
        Coffee? result = null;
        
        if (reader.Read())
        {
            result = CoffeeFromReader(reader);
        }
        
        return result;
    }

    public bool Insert(Coffee coffee)
    {
        using var conn = Connection;
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = _coffeeInsert;
        cmd.Parameters.AddWithValue("@title", coffee.Title);
        cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

        coffee.Id = (int)cmd.ExecuteScalar();
        return coffee.Id != 0;
    }

    public bool Update(Coffee coffee)
    {
        using var conn = Connection;
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = _coffeeUpdate;
        cmd.Parameters.AddWithValue("@id", coffee.Id);
        cmd.Parameters.AddWithValue("@title", coffee.Title);
        cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

        int rowsAffected = cmd.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public bool Delete(int id)
    {
        using var conn = Connection;
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = _coffeeDelete;

        return cmd.ExecuteNonQuery() > 0;
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
