using ApiCrud.Data.CustomModels;

namespace ApiCrud.Services.IServices;

public interface IApiCrudService
{
    public IEnumerable<BookViewModel> GetAllBooks();
    public BookCrudResponseModel AddBook(BookViewModel book);
    public BookCrudResponseModel DeleteBook(int id);
    public BookCrudResponseModel UpdateBook(BookViewModel book);
    public string Authenticate(LoginDetails model);
}
