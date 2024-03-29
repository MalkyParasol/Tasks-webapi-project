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
    //private readonly PasswordHasher<TUser> _identityHasher;
    private readonly ImprovedPasswordHasherOptions _options;

    /// <summary>
    /// Creates a new instance of <see cref="PasswordHasher{TUser}"/>.
    /// </summary>
    /// <param name="identityHasher">AspNet Identity PasswordHasher</param>
    /// <param name="optionsAccessor">The options for this instance.</param>
    public PasswordHasherService(IOptions<ImprovedPasswordHasherOptions>? optionsAccessor = null)
    {
        //PasswordHasher<TUser> identityHasher,
        //_identityHasher = identityHasher;
        _options = optionsAccessor?.Value ?? new ImprovedPasswordHasherOptions();
        //optionsAccessor?.Value ?? new ImprovedPasswordHasherOptions();
    }

    public string HashPassword(string password)
    //TUser user,
    {
        ArgumentNullException.ThrowIfNull(password);
        //  ArgumentNullException.ThrowIfNull(user);

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

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {

        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(providedPassword);

        var info = new HashInfo(hashedPassword);
        //if (info.IsAspNetV2 || info.IsAspNetV3)
        //{
        //    var result = _identityHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        //    if (result == PasswordVerificationResult.Success ||
        //        result == PasswordVerificationResult.SuccessRehashNeeded)
        //        return PasswordVerificationResult.SuccessRehashNeeded;

        //    return PasswordVerificationResult.Failed;
        //}

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