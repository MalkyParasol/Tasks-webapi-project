

using Microsoft.AspNetCore.Identity;

public interface IPasswordHasher<TUser>
{
    /// <summary>
    /// Hashes the password for the specified user.
    /// </summary>
    /// <param name="user">The user for which to hash the password.</param>
    /// <param name="password">The password to hash.</param>
    /// <returns>A hashed representation of the password for the user.</returns>
    string HashPassword( string password);
    //TUser user,

    /// <summary>
    /// Verifies a password against a hashed password.
    /// </summary>
    /// <param name="user">The user associated with the hashed password.</param>
    /// <param name="hashedPassword">The hashed password to verify against.</param>
    /// <param name="providedPassword">The password provided for verification.</param>
    /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of the verification.</returns>
    PasswordVerificationResult VerifyHashedPassword( string hashedPassword, string providedPassword);
}