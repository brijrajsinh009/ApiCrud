using ApiCrud.Data.Models;

namespace ApiCrud.Data.IRepo;

public interface IUserRepo
{
    public User User(string email);
    public bool AddUser(User model);
}
