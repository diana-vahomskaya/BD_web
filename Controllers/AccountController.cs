using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Workers.Models;
using Workers.Service;

namespace Workers.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;

        public BdBrain Bd { get; }

        public AccountController(BdBrain _Bd, ILogger<AccountController> logger)
        {
            Bd = _Bd;
            _logger = logger;

            _logger.LogDebug("Controller AccountController.");
        }

        
        [HttpGet]
        public IActionResult Authorization() => View();

        [HttpPost]
        public async Task<IActionResult> Authorization(WorkersModel workers)
        {
           
            foreach (var item in Bd.GetWorkers())
                if (item.Login == workers.Login && item.Password == workers.Password)
                {
                    _logger.LogInformation("Authorization done", nameof(workers));

                    await Authentication(Bd.GetLogin(workers.Login));

                    return RedirectToAction("Index", "WorkersModels");
                }

            _logger.LogWarning("Authorization not done", nameof(workers));

            return View();
        }
       
        [HttpGet]
        public IActionResult Registration() => View();

        [HttpPost]
        public async Task<IActionResult> Registration(WorkersModel workers)
        {
            if (workers == null) throw new ArgumentNullException("Date can not be empty", nameof(workers));

            if (!ModelState.IsValid)
            {
                return View();
            }
            
            Bd.Create(workers);

            await Authentication(Bd.GetLogin(workers.Login));

            return RedirectToAction("Index", "WorkersModels");
        }
        

        private async Task Authentication(WorkersModel workers)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, workers.Login),
                    new Claim(ClaimTypes.Role, workers.Role)
                };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimTypes.Role);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
       
    }
}
