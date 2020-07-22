using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Авторизация
        [HttpGet]
        public IActionResult Authorization() => View();

        [HttpPost]
        public async Task<IActionResult> Authorization(WorkersModel workers)
        {
            _logger.LogInformation("Данные пользователя получены.", nameof(workers));

            foreach (var item in Bd.GetWorkers())
                if (item.Login == workers.Login && item.Password == workers.Password)
                {
                    _logger.LogInformation("Авторизация выполнена успешно.", nameof(workers));

                    await Authentication(workers);

                    return RedirectToAction("Index", "WorkersModel");
                }

            _logger.LogWarning("Авторизация не выполнена.", nameof(workers));

            return View();
        }
        #endregion

        #region Регистрация
        [HttpGet]
        public IActionResult Registration() => View();

        [HttpPost]
        public async Task<IActionResult> Registration(WorkersModel workers)
        {
            _logger.LogInformation("Данные пользователя получены.", nameof(workers));

            if (workers == null) throw new ArgumentNullException("Данные пользователя не могут быть пустыми.", nameof(workers));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не все текстовые поля заполнены.");

                return View();
            }

            Bd.Create(workers);

            await Authentication(workers);

            return RedirectToAction("Index", "WorkersModels");
        }
        #endregion

        private async Task Authentication(WorkersModel workers)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, workers.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, workers.Role.ToString())
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
       
    }
}
