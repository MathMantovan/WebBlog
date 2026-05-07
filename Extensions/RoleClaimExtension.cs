using Blog.Models;
using System.Security.Claims;

namespace Blog.Extensions
{
    public static class RoleClaimExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new (ClaimTypes.Name, user.Email)
            };
            result.AddRange(user.Roles.Select(Role => new Claim(ClaimTypes.Role, Role.Slug)));

            return result;
        }
    }
}
