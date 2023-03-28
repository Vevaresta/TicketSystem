using Microsoft.AspNetCore.Identity;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Validators;

public class CustomPasswordValidator : IPasswordValidator<User>
{
    #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
    {
        var errors = new List<IdentityError>();

        if (!password.Any(char.IsNumber))
        {
            errors.Add(new IdentityError { Description = "Das Passwort muss mindestens eine Zahl enthalten ('0'-'9')." });
        }

        if (!password.Any(char.IsLower))
        {
            errors.Add(new IdentityError { Description = "Das Passwort muss mindestens einen Kleinbuchstaben enthalten ('a'-'z')." });
        }

        if (!password.Any(char.IsUpper))
        {
            errors.Add(new IdentityError { Description = "Das Passwort muss mindestens einen Großbuchstaben enthalten ('A'-'Z')." });
        }

        if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation))
        {
            errors.Add(new IdentityError { Description = "Das Passwort muss mindestens ein Sonderzeichen enthalten." });
        }

        if (errors.Count > 0)
        {
            return IdentityResult.Failed(errors.ToArray());
        }

        return IdentityResult.Success;
    }
}
