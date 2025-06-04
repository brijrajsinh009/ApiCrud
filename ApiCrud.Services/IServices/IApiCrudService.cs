using ApiCrud.Data.CustomModels;

namespace ApiCrud.Services.IServices;

public interface IApiCrudService
{
    public IEnumerable<BookViewModel> GetAllBooks();

    public (bool, int, string) AddBook(BookViewModel book);
    public (bool, string) DeleteBook(int id);
    public (bool, int, string) UpdateBook(BookViewModel book);
    public (bool, string) Authenticate(LoginDetails model);




}
