using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        List<Coffee> GetAll();
        Coffee? GetById(int id);
    }
}