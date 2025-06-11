using System.Data;
using System.Security.Claims;
using ApiCrud.Data.CustomModels;
using ApiCrud.Services.Attributes;
using ApiCrud.Services.IServices;
using ApiCrud.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiCrud.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiCrudController : ControllerBase
{

    private readonly IApiCrudService _apiCRUDService;
    private readonly ITokenService _tokenService;
    ApiResponseModel responseModel = new ApiResponseModel { };

    public ApiCrudController(IApiCrudService apiCRUDService, ITokenService tokenService)
    {
        _apiCRUDService = apiCRUDService;
        _tokenService = tokenService;
    }


    [HttpPost("Login", Name = "Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginDetails model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                responseModel.Message = "Model state is not valid.";
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            string token = _apiCRUDService.Authenticate(model);
            string[] tokens = new string[2];
            tokens[0] = token;
            tokens[1] = _tokenService.GenerateRefreshToken(model.UserEmail);
            responseModel.Data = tokens;
            _tokenService.SaveJWTToken(Response, token);
            _tokenService.SaveRefreshJWTToken(Response, tokens[1]);
            responseModel.Message = "Logged in";
            responseModel.Success = true;
            return Ok(responseModel);
        }
        catch (UnauthorizedAccessException ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status401Unauthorized, responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }


    [HttpGet("Books", Name = "GetBooks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [CutomAuth]
    public IActionResult GetBooks()
    {
        try
        {
            IEnumerable<BookViewModel> books = _apiCRUDService.GetAllBooks();
            if (books.Count() == 0)
            {
                responseModel.Message = "No Books.";
                responseModel.Success = false;
                return NotFound(responseModel);
            }
            responseModel.Message = "All Books.";
            responseModel.Success = true;
            responseModel.Data = books;
            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }


    [HttpGet("Book", Name = "GetBook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [CutomAuth]
    public IActionResult GetBook(int id)
    {
        try
        {
            if (id <= 0)
            {
                responseModel.Message = "Id not valid!";
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            BookCrudResponseModel response = _apiCRUDService.GetBook(id);
            responseModel.Message = response.Message;
            responseModel.Success = true;
            responseModel.Data = response.Data;
            return Ok(responseModel);
        }
        catch (KeyNotFoundException ex)
        {
            responseModel.Message = "No Books.";
            responseModel.Success = false;
            return NotFound(responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }


    [HttpPost("AddBook", Name = "AddBook")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [CutomAuth]
    public IActionResult AddBook([FromForm] BookViewModel newBook)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                responseModel.Message = "Model state is not valid.";
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            if (newBook.Id != 0)
            {
                responseModel.Message = "Id not valid!";
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            BookCrudResponseModel response = _apiCRUDService.AddBook(newBook);
            responseModel.Message = response.Message;
            responseModel.Success = true;
            responseModel.Data = response.Id;
            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }


    [HttpDelete("DeleteBook", Name = "DeleteBook")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [CutomAuth]
    public IActionResult DeleteBook(int id)
    {
        try
        {
            if (id <= 0)
            {
                responseModel.Message = "Id not valid!";
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            BookCrudResponseModel response = _apiCRUDService.DeleteBook(id);
            responseModel.Message = response.Message;
            responseModel.Success = true;
            return Ok(responseModel);
        }
        catch (KeyNotFoundException ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return NotFound(responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }


    [HttpPost("UpdateBook", Name = "UpdateBook")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [CutomAuth]
    public IActionResult UpdateBook([FromBody] BookViewModel book)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                responseModel.Message = "Model state is not valid.";
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            if (book.Id <= 0)
            {
                responseModel.Message = "Id not valid!";
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            BookCrudResponseModel response = _apiCRUDService.UpdateBook(book);
            responseModel.Message = response.Message;
            responseModel.Success = true;
            return Ok(responseModel);
        }
        catch (KeyNotFoundException ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return NotFound(responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }


    [HttpPost("Register", Name = "Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Register([FromForm] UserViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                responseModel.Message = string.Join("; ", errors);
                responseModel.Success = false;
                return BadRequest(responseModel);
            }
            BookCrudResponseModel response = _apiCRUDService.Registration(model);
            responseModel.Message = response.Message;
            responseModel.Success = true;
            responseModel.Data = response.Id;
            return Ok(responseModel);
        }
        catch (DuplicateNameException ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status400BadRequest, responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }


    [HttpPost("Tokens", Name = "Tokens")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Tokens([FromBody] string token)
    {
        try
        {
            ClaimsPrincipal principal = _tokenService.ValidateRefreshToken(token);
            if (principal == null)
            {
                throw new UnauthorizedAccessException("invalide token!");
            }
            string email = _tokenService.GetEmailFromToken(token);
            string[] tokens = new string[2];
            tokens[0] = _apiCRUDService.AccessToken(email);
            tokens[1] = _tokenService.GenerateRefreshToken(email);
            responseModel.Message = "Tokens";
            responseModel.Success = true;
            responseModel.Data = tokens;
            return Ok(responseModel);
        }
        catch (UnauthorizedAccessException ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status401Unauthorized, responseModel);
        }
        catch (Exception ex)
        {
            responseModel.Message = ex.Message;
            responseModel.Success = false;
            return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
        }
    }
}
