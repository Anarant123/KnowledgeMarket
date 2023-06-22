using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KnowledgeMarketWebAPI.Domain.Enums;
using KnowledgeMarketWebAPI.Auth;
using KnowledgeMarketWebAPI.Contracts.Requests.Models;
using KnowledgeMarketWebAPI.Contracts.Responses;
using KnowledgeMarketWebAPI.Data;
using KnowledgeMarketWebAPI.Data.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using AdBoardsWebAPI.Options;
using KnowledgeMarketWebAPI.Data.Models.db;

namespace KnowledgeMarketWebAPI.Extensions;

public static class UserEndpoints
{

    private static async Task<Error?> ValidateEmail(string email, KnowledgeMarketContext context)
    {
        if (await context.Users.AnyAsync(x => x.Email == email))
        {
            return new Error
            {
                Reason = "email_unique",
                Message = "Указанный email уже занят"
            };
        }

        return null;
    }

    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("User").WithTags("User");

        group.MapGet("GetMe", async (KnowledgeMarketContext context, ClaimsPrincipal user) =>
        {
            var id = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            return await context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
        });

        group.MapGet("Authorization", async (string login, string password, KnowledgeMarketContext context,
            IOptions<JwtOptions> jwtOptions) =>
        {
            var user = await context.Users
                .Include(x => x.Role).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (user is null) return Results.BadRequest();

            var key = Encoding.ASCII.GetBytes(jwtOptions.Value.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim("email", user.Email),
                    new Claim("name", user.Name),
                    new Claim("login", user.Login),
                    new Claim("roleId", user.RoleId.ToString())
                }, "jwt"),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtOptions.Value.Issuer,
                Audience = jwtOptions.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return Results.Ok(new { user, Token = stringToken });
        }).AllowAnonymous();

        group.MapPost("Registration", async (RegisterModel model, KnowledgeMarketContext context, FileManager fileManager) =>
        {
            var p = new User
            {
                Login = model.Login.Trim(),
                Password = model.Password,
                Name = model.Name?.Trim(),
                Email = model.Email.Trim(),
                RoleId = (int?)RoleType.Normal,
            };

            var errors = new List<Error>(3);

            if (await context.Users.AnyAsync(x => x.Login == p.Login))
            {
                errors.Add(new Error
                {
                    Reason = "login_unique",
                    Message = "Указанный логин уже занят"
                });
            }

            var emailValidationResult = await ValidateEmail(p.Email, context);
            if (emailValidationResult is not null) errors.Add(emailValidationResult);

            if (errors.Count != 0) return Results.BadRequest(errors);

            context.Users.Add(p);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return Results.BadRequest();
            }

            return Results.Ok(p);
        }).AllowAnonymous();

        group.MapDelete("Delete", async (string login, KnowledgeMarketContext context) =>
        {
            var p = await context.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (p is null) return Results.NotFound();

            context.Users.Remove(p);
            await context.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization(Policies.Admin);

        return app;
    }
}