using ApiCrud.Data.Models;

namespace ApiCrud.Data.IRepo;

public interface IBooksRepo
{
    public IEnumerable<Book> Books();
    public bool AddBook(Book book);
    public bool UpdateBook(Book book);
    public Book Book(int id);




}
