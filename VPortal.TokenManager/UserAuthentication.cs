// using System.Security.Claims;
// using System.Security.Principal;
// using System.Threading.Tasks;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Options;
// using Newtonsoft.Json;

namespace VPortal.TokenManager
{
    public  class UserAuthentication
    {
    //  public  Task<ClaimsIdentity> IdentityResolver { get; set; }
        
    //     public  bool isValidUser(string userName, string password)
    //     {
    //         var identity = await IdentityResolver(userName, password);
    //         if (identity == null)
    //         {
    //             context.Response.StatusCode = 400;
    //             await context.Response.WriteAsync("Invalid username or password.");
    //             return false;
    //         }
    //         else
    //         {
    //             GenerateToken(userName,password);
    //             IdentityResolver = GetIdentity(userName,password);
    //             return true;
    //         }
    //     }

    //     private Task<ClaimsIdentity> GetIdentity(string username, string password)
    //     {
    //         // DEMO CODE, DON NOT USE IN PRODUCTION!!!
    //         if (username == "TEST" && password == "TEST123")
    //         {
    //             return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
    //         }

    //         // Account doesn't exists
    //         return Task.FromResult<ClaimsIdentity>(null);
    //     }
        
    }
}