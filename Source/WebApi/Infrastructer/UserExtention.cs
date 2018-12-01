// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SessionController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Infrastructer
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using WebApi.EF.Models;
    using WebApi.Models;

    /// <summary>
    /// The user extention.
    /// </summary>
    public static class UserExtention
    {
        /// <summary>
        /// The get user from db in context async.
        /// </summary>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static Task<User> GetUserFromDbInContextAsync(this ClaimsPrincipal claims, EFContext context)
        {
            var userInfo = new UserInfo
                               {
                                   Inn = claims.FindFirst("Inn").Value,
                                   Login = claims.FindFirst("Login").Value
                               };

            return (from u in context.Users where u.INN == userInfo.Inn || u.Login == userInfo.Login select u)
                .FirstOrDefaultAsync();
        }

    }
}