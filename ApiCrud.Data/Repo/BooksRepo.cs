using ApiCrud.Data.Context;
using ApiCrud.Data.IRepo;
using ApiCrud.Data.Models;

namespace ApiCrud.Data.Repo;

public class BooksRepo : IBooksRepo
{
    private readonly ApiCrudContext _context;

    public BooksRepo(ApiCrudContext context)
    {
        _context = context;
    }

    public IEnumerable<Book> Books()
    {
        return _context.Books.Where(b => b.IsDelete == false).ToList();
    }


    public bool AddBook(Book book)
    {
        _context.Add(book);
        return _context.SaveChanges() > 0;
    }


    public bool UpdateBook(Book book)
    {
        _context.Update(book);
        return _context.SaveChanges() > 0;
    }


    public Book Book(int id)
    {
        return _context.Books.FirstOrDefault(b => b.Id == id);
    }


}
