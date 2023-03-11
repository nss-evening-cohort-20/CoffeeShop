using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        List<Coffee> GetAll();
        Coffee? GetById(int id);
        bool Insert(Coffee coffee);
        bool Update(Coffee coffee);
        bool Delete(int id);
    }
}