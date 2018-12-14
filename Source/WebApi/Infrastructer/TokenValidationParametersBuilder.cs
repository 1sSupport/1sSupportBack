// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenValidationParametersBuilder.cs" company="">
//   
// </copyright>
// <summary>
//   The token validation parameters builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Infrastructer
{
    using System;
    using System.Text;

    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///     The token validation parameters builder.
    /// </summary>
    public static class TokenValidationParametersBuilder
    {
        /// <summary>
        /// The get token validation parameters.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <returns>
        /// The <see cref="TokenValidationParameters"/>.
        /// </returns>
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ClockSkew = TimeSpan.FromMinutes(0),
                           ValidateLifetime = true,
                           ValidIssuer = $"{configuration["JWT:issuer"]}",
                           ValidAudience = $"{configuration["JWT:audience"]}",
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]))
                       };
        }
    }
}