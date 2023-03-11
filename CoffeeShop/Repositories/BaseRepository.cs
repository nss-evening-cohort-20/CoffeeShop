using Microsoft.Data.SqlClient;

namespace CoffeeShop.Repositories;

public abstract class BaseRepository
{
    private readonly string _connectionString;

    public BaseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    protected SqlConnection Connection => new(_connectionString);

}
