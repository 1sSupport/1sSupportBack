// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenValidationParametersBuilder.cs" company="">
//
// </copyright>
// <summary>
//   Defines the TokenValidationParametersBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Infrastructer
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    /// <summary>
    ///     The token validation parameters builder.
    /// </summary>
    public static class TokenValidationParametersBuilder
    {
        /// <summary>
        ///     The get token validation parameters.
        /// </summary>
        /// <param name="configuration">
        ///     The configuration.
        /// </param>
        /// <returns>
        ///     The <see cref="TokenValidationParameters" />.
        /// </returns>
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,

                           // ValidateLifetime = true,
                           ValidIssuer = $"{configuration["JWT:issuer"]}",
                           ValidAudience = $"{configuration["JWT:audience"]}",
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]))
                       };
        }
    }
}