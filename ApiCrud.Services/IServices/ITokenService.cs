namespace ApiCrud.Services.IServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public interface ITokenService
{
    public string GenerateJwtToken(string name, string email, int id);
    public void SaveJWTToken(HttpResponse response, string token);
    public void SaveRefreshJWTToken(HttpResponse response, string token);
    public string? GetJWTToken(HttpRequest request);
    public ClaimsPrincipal? ValidateToken(string token);
    public string GenerateRefreshToken(string email);
    public ClaimsPrincipal? ValidateRefreshToken(string token);
    public string? GetEmailFromToken(string token);
    
    

}
