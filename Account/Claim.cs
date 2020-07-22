using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
