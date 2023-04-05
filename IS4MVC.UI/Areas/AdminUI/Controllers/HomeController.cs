using IS4MVC.Database;
using IS4MVC.UI.Configuration.Constants;
using IS4MVC.UI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS4MVC.UI.Areas.AdminUI.Controllers
{
    [Area(CommonConsts.AdminUIArea)]
    public class HomeController:Controller
    {
        private readonly AppDbContext _ctx;

        public HomeController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            var d = _ctx.TestTable.ToList();
            return View(d);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectTheme(string theme, string returnUrl)
        {
            Response.Cookies.Append(
                ThemeHelpers.CookieThemeKey,
                theme,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
