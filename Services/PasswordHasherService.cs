using NetDevPack.Security.PasswordHasher.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sodium;
using System;
using NetDevPack.Security.PasswordHasher.Core.Utilities;
using MyTasks.Interfaces;
using MyTasks.Models;

namespace MyTasks.Services;

public class PasswordHasherService<TUser> : IPasswordHasher<TUser> where TUser : class
{
    private readonly ImprovedPasswordHasherOptions _options;

    /// <summary>
    /// Creates a new instance of <see cref="PasswordHasher{TUser}"/>.
    /// </summary>
    /// <param name="optionsAccessor">The options for this instance.</param>
    public PasswordHasherService(IOptions<ImprovedPasswordHasherOptions>? optionsAccessor = null)
    {
        _options = optionsAccessor?.Value ?? new ImprovedPasswordHasherOptions();
    }

    public string HashPassword(string password)
    {
        ArgumentNullException.ThrowIfNull(password);

        if (_options.OpsLimit.HasValue && _options.MemLimit.HasValue)
            return PasswordHash.ArgonHashString(password, _options.OpsLimit.Value, _options.MemLimit.Value);

        // Removing trailing 0x00. Some database providers doesnt support it.:
        // https://github.com/npgsql/efcore.pg/issues/1069
        return _options.Strenght switch
        {
            PasswordHasherStrenght.Interactive => PasswordHash.ArgonHashString(password).Replace("\0", string.Empty),
            PasswordHasherStrenght.Moderate => PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Moderate).Replace("\0", string.Empty),
            PasswordHasherStrenght.Sensitive => PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Sensitive).Replace("\0", string.Empty),
            _ => throw new ArgumentOutOfRangeException()
        };

    }

    public PasswordVerificationResult VerifyHashedPassword( string hashedPassword, string providedPassword)
    {
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(providedPassword);

        var info = new HashInfo(hashedPassword);
        return PasswordHash.ArgonHashStringVerify(hashedPassword, providedPassword)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }


}

public static class PasswordHasherUtils
{
    public static void AddPasswordHasher(this IServiceCollection services)
    {
        services.AddTransient<IPasswordHasher<User>, PasswordHasherService<User>>();
    }
}