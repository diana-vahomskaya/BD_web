using Microsoft.AspNetCore.Authorization;

namespace Workers.Account
{
    public class Claim : IAuthorizationRequirement
    {
        protected internal string Role { get; }

        public Claim(string role)
        {
            Role = role;
        }
    }
}
