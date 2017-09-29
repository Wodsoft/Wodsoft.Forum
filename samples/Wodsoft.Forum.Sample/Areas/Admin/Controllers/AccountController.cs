using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Mvc;
using Wodsoft.ComBoost.Security;
using Wodsoft.Forum.Sample.Domain;
using Wodsoft.Forum.Sample.Entity;

namespace Wodsoft.Forum.Sample.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : DomainController
    {
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string username)
        {
            if (User.Identity.IsAuthenticated)
                return StatusCode(200);
            var memberDomain = DomainProvider.GetService<MemberDomainService>();
            var context = CreateDomainContext();
            try
            {
                await memberDomain.ExecuteAsync(context, "SignIn");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            return StatusCode(200);
        }

        public async Task<IActionResult> SignOut()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            var memberDomain = DomainProvider.GetService<MemberDomainService>();
            var context = CreateDomainContext();
            await memberDomain.ExecuteAsync(context, "SignOut");
            if (Request.Query["returnUrl"].Count > 0)
                return Redirect(Request.Query["returnUrl"][0]);
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
