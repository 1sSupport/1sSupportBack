// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TokenController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    using WebApi.EF.Models;
    using WebApi.Models;

    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     The token controller.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        /// <summary>
        ///     The Context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        ///     The configuration.
        /// </summary>
        private readonly TokenValidationParameters tokenParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="tokenParameters">
        /// The token Parameters.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public TokenController(TokenValidationParameters tokenParameters, EFContext context)
        {
            this.tokenParameters = tokenParameters;
            this.context = context;
        }

        /// <summary>
        /// The create token.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateToken(UserInfo info)
        {
            if (!this.ModelState.IsValid) return this.NoContent();

            IActionResult response = this.Unauthorized();

            var identity = await this.GetUserIdentity(info).ConfigureAwait(false);

            if (identity == null) return response;

            var token = await this.JwtTokenBuilderAsync(identity.Claims).ConfigureAwait(false);
            response = this.Ok(new { access_token = token });
            return response;
        }

        /// <summary>
        /// The get user identity.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<ClaimsIdentity> GetUserIdentity(UserInfo info)
        {
            var user = await (from u in this.context.Users
                              where string.Equals(u.INN, info.Inn, StringComparison.OrdinalIgnoreCase) && string.Equals(
                                        u.Login,
                                        info.Login,
                                        StringComparison.OrdinalIgnoreCase)
                              select new { u.INN, u.Login }).FirstOrDefaultAsync().ConfigureAwait(false);

            if (user == null) return null;

            var claims = new List<Claim> { new Claim("Login", user.Login), new Claim("Inn", user.INN) };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                "JWT",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        /// ///
        /// <summary>
        /// The jwt token builder.
        /// </summary>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private Task<string> JwtTokenBuilderAsync(IEnumerable<Claim> claims)
        {
            return Task.Run(
                () =>
                    {
                        var now = DateTime.UtcNow;
                        var key = this.tokenParameters.IssuerSigningKey;
                        var credantials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var handler = new JwtSecurityTokenHandler();
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: this.tokenParameters.ValidIssuer,
                            audience: this.tokenParameters.ValidAudience,
                            signingCredentials: credantials,
                            notBefore: now,
                            expires: now.AddMinutes(15));
                        return new JwtSecurityTokenHandler().WriteToken(token);
                    });
        }
    }
}