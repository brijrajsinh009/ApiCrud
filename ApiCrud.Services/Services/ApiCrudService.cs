using ApiCrud.Data.CustomModels;
using ApiCrud.Data.IRepo;
using ApiCrud.Data.Models;
using ApiCrud.Services.IServices;
using AutoMapper;

namespace ApiCrud.Services.Services;

public class ApiCrudService : IApiCrudService
{
    private readonly IBooksRepo _bookRepo;
    private readonly IUserRepo _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public ApiCrudService(IBooksRepo bookRepo, IUserRepo userRepo, ITokenService tokenService, IMapper mapper)
    {
        _bookRepo = bookRepo;
        _userRepo = userRepo;
        _tokenService = tokenService;
        _mapper = mapper;
    }


    public IEnumerable<BookViewModel> GetAllBooks()
    {
        IEnumerable<Book> booksDB = _bookRepo.Books();
        IEnumerable<BookViewModel> books = _mapper.Map<IEnumerable<BookViewModel>>(booksDB);
        return books;
    }


    public BookCrudResponseModel AddBook(BookViewModel book)
    {
        Book model = _mapper.Map<Book>(book);
        model.CreatedOn = DateTime.Now;
        model.ModifiedOn = DateTime.Now;
        if (!_bookRepo.AddBook(model))
        {
            throw new InvalidOperationException("Book Not Added!");
        }
        BookCrudResponseModel response = new BookCrudResponseModel
        {
            Id = model.Id,
            Message = "Book Added!"
        };
        return response;
    }


    public BookCrudResponseModel DeleteBook(int id)
    {
        Book model = _bookRepo.Book(id);
        if (model == null || model.Id == 0)
        {
            throw new KeyNotFoundException("Book not found!");
        }
        model.IsDelete = true;
        model.ModifiedOn = DateTime.Now;
        if (!_bookRepo.UpdateBook(model))
        {
            throw new InvalidOperationException("Book not deleted!");
        }
        BookCrudResponseModel response = new BookCrudResponseModel
        {
            Id = model.Id,
            Message = "Book deleted!"
        };
        return response;
    }


    public BookCrudResponseModel UpdateBook(BookViewModel book)
    {
        Book model = _bookRepo.Book(book.Id);
        if (model == null || model.Id == 0)
        {
            throw new KeyNotFoundException("Book not found!");
        }
        model.Name = book.Name;
        model.Author = book.Author;
        model.Price = book.Price;
        model.ModifiedOn = DateTime.Now;
        if (!_bookRepo.UpdateBook(model))
        {
            throw new InvalidOperationException("Book not edited!");
        }
        BookCrudResponseModel response = new BookCrudResponseModel
        {
            Id = model.Id,
            Message = "Book edited!"
        };
        return response;
    }


    public BookCrudResponseModel GetBook(int id)
    {
        Book model = _bookRepo.Book(id);
        if (model == null || model.Id == 0)
        {
            throw new KeyNotFoundException("Book not found!");
        }
        BookCrudResponseModel response = new BookCrudResponseModel
        {
            Id = model.Id,
            Message = "Book!",
            Data = model
        };
        return response;
    }



    public string Authenticate(LoginDetails model)
    {
        User user = _userRepo.User(model.UserEmail);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Email not found.");
        }
        if (user.Password != model.Password)
        {
            throw new UnauthorizedAccessException("Password does not match.");
        }
        return _tokenService.GenerateJwtToken(user.Name, user.Email, user.Id);
    }
}
