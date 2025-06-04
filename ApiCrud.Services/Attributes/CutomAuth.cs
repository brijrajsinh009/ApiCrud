using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ApiCrud.Services.IServices;
using ApiCrud.Data.IRepo;
using ApiCrud.Data.Models;

namespace ApiCrud.Services.Attributes;

public class CutomAuth : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var jwtService = context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as ITokenService;
        var userData = context.HttpContext.RequestServices.GetService(typeof(IUserRepo)) as IUserRepo;

        var token = jwtService.GetJWTToken(context.HttpContext.Request);
        ClaimsPrincipal principal = null;
        try
        {
            principal = jwtService.ValidateToken(token ?? "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        }
        catch
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        if (principal == null)
        {
            var refreshToken = context.HttpContext.Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            ClaimsPrincipal principalRefresh;
            try
            {
                principalRefresh = jwtService.ValidateRefreshToken(refreshToken);
            }
            catch
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (principalRefresh == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var emailInRefreshToken = jwtService.GetEmailFromToken(refreshToken);
            if (string.IsNullOrEmpty(emailInRefreshToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            User user = userData.User(emailInRefreshToken);
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            token = jwtService.GenerateJwtToken(user.Name, user.Email,user.Id);
            jwtService.SaveJWTToken(context.HttpContext.Response, token);
            refreshToken = jwtService.GenerateRefreshToken(user.Email);
            jwtService.SaveRefreshJWTToken(context.HttpContext.Response, refreshToken);
            try
            {
                principal = jwtService.ValidateToken(token ?? "");
            }
            catch
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (principal == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

        }
        context.HttpContext.User = principal;
    }
}

