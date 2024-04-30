

using Microsoft.AspNetCore.Identity;

public interface IPasswordHasher<TUser>
{
    /// <summary>
    /// Hashes the password for the specified user.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A hashed representation of the password for the user.</returns>
    string HashPassword( string password);

    /// <summary>
    /// Verifies a password against a hashed password.
    /// </summary>
    /// <param name="hashedPassword">The hashed password to verify against.</param>
    /// <param name="providedPassword">The password provided for verification.</param>
    /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of the verification.</returns>
    PasswordVerificationResult VerifyHashedPassword( string hashedPassword, string providedPassword);
}