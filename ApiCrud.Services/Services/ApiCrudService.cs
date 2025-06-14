using System.Data;
using ApiCrud.Data.CustomModels;
using ApiCrud.Data.IRepo;
using ApiCrud.Data.Models;
using ApiCrud.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;

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
        // string itemImagePath = null;
        // if (book.ImageFile != null && book.ImageFile.Length > 0)
        // {
        //     itemImagePath = UploadFile(book.ImageFile,"books");
        //     if (itemImagePath == null)
        //     {
        //         Console.WriteLine("Failed to upload");
        //     }
        // }
        Book model = _mapper.Map<Book>(book);
        model.CreatedOn = DateTime.Now;
        model.ModifiedOn = DateTime.Now;
        // if (itemImagePath != null)
        // {
        //     model.Image=itemImagePath;
        // }
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



    public BookCrudResponseModel Registration(UserViewModel model)
    {
        string itemImagePath = null;
        if (model.Image != null && model.Image.Length > 0)
        {
            itemImagePath = UploadFile(model.Image,"profiles");
            if (itemImagePath == null)
            {
                Console.WriteLine("Failed to upload");
            }
        }
        User oldUser = _userRepo.User(model.UserEmail);
        if (oldUser != null)
        {
            throw new DuplicateNameException("Email Already Exist!");
        }
        User user = _mapper.Map<User>(model);
        user.CreatedOn = DateTime.Now;
        user.ModifiedOn = DateTime.Now;
        user.IsDelete = false;
        if (itemImagePath != null)
        {
            user.ProfileUrl=itemImagePath;
        }
        if (!_userRepo.AddUser(user))
        {
            throw new InvalidOperationException("User Not Added!");
        }
        BookCrudResponseModel response = new BookCrudResponseModel
        {
            Id = user.Id,
            Message = "User Added!",
            Data = user
        };
        return response;
    }



    public string AccessToken(string mail)
    {
        User user = _userRepo.User(mail);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Email not found.");
        }
        return _tokenService.GenerateJwtToken(user.Name, user.Email, user.Id);
    }


    private string? UploadFile(IFormFile file,string path)
    {
        try
        {
            if (file != null && file.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{path}");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    file.CopyToAsync(fileStream);
                }
                return Path.Combine($"images/{path}", fileName).Replace("\\", "/");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"File upload error: {e.Message}");
            return null;
        }
        return null;
    }
}
