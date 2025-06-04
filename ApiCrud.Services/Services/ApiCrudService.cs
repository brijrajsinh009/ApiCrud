using ApiCrud.Data.CustomModels;
using ApiCrud.Data.IRepo;
using ApiCrud.Data.Models;
using ApiCrud.Services.IServices;

namespace ApiCrud.Services.Services;

public class ApiCrudService : IApiCrudService
{
    private readonly IBooksRepo _bookRepo;
    private readonly IUserRepo _userRepo;
    private readonly ITokenService _tokenService;

    public ApiCrudService(IBooksRepo bookRepo, IUserRepo userRepo,ITokenService tokenService)
    {
        _bookRepo = bookRepo;
        _userRepo = userRepo;
        _tokenService = tokenService;
    }

    public IEnumerable<BookViewModel> GetAllBooks()
    {
        IEnumerable<Book> booksDB = _bookRepo.Books();
        IEnumerable<BookViewModel> books = Enumerable.Empty<BookViewModel>();
        foreach (Book bookDB in booksDB)
        {
            BookViewModel book = new BookViewModel
            {
                Id = bookDB.Id,
                Name = bookDB.Name,
                Author = bookDB.Author,
                Price = bookDB.Price
            };
            books = books.Append(book).ToList();
        }

        return books;
    }



    public (bool, int, string) AddBook(BookViewModel book)
    {
        Book model = new Book
        {
            Name = book.Name,
            Author = book.Author,
            Price = book.Price,
            IsDelete = false,
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now,
        };
        if (_bookRepo.AddBook(model))
        {
            return (true, model.Id, "Book Added!");
        }
        else
            return (false, 0, "Book Not Added!");
    }



    public (bool, string) DeleteBook(int id)
    {
        Book model = _bookRepo.Book(id);
        if (model == null || model.Id == 0)
        {
            return (false, "Book Not Found!");
        }
        model.IsDelete = true;
        model.ModifiedOn = DateTime.Now;
        if (_bookRepo.UpdateBook(model))
        {
            return (true, "Book Deleted!");
        }
        else
            return (false, "Book Not Deleted!");
    }


    public (bool, int, string) UpdateBook(BookViewModel book)
    {
        Book model = _bookRepo.Book(book.Id);
        if (model == null || model.Id == 0)
        {
            return (false, book.Id, "Book Not Found!");
        }
        model.Name = book.Name;
        model.Author = book.Author;
        model.Price = book.Price;
        model.ModifiedOn = DateTime.Now;
        if (_bookRepo.UpdateBook(model))
        {
            return (true, book.Id, "Book Updated!");
        }
        else
            return (false, book.Id, "Book Not Updated!");
    }


    public (bool, string) Authenticate(LoginDetails model)
    {
        User user = _userRepo.User(model.UserEmail);
        if(user==null)
        {
            return (false, "Email not Found.");
        }
        if(user.Password!=model.Password)
        {
            return (false, "Password not match.");
        }
        return (true,_tokenService.GenerateJwtToken(user.Name,user.Email,user.Id));
    }
}
