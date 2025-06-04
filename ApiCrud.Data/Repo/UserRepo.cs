using ApiCrud.Data.Context;
using ApiCrud.Data.IRepo;
using ApiCrud.Data.Models;

namespace ApiCrud.Data.Repo;

public class UserRepo : IUserRepo
{
    private readonly ApiCrudContext _context;

    public UserRepo(ApiCrudContext context)
    {
        _context = context;
    }


    public User User(string email)
    {
        return _context.Users.FirstOrDefault(b => b.Email == email);
    }
}
