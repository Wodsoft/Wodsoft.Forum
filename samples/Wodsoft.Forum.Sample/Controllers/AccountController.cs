using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wodsoft.ComBoost.Mvc;
using Wodsoft.Forum.Sample.Domain;
using Wodsoft.ComBoost;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Wodsoft.Forum.Sample.Entity;

namespace Wodsoft.Forum.Sample.Controllers
{
    public class AccountController : DomainController
    {
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Board");
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
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is UnauthorizedAccessException)
                    return StatusCode(401, ex.InnerException.Message);
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
            return StatusCode(200);
        }

        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username)
        {
            if (User.Identity.IsAuthenticated)
                return StatusCode(200);
            var memberDomain = DomainProvider.GetService<MemberDomainService>();
            var context = CreateDomainContext();
            try
            {
                await memberDomain.ExecuteAsync(context, "SignUp");
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is ArgumentException)
                    return StatusCode(401, ex.InnerException.Message);
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
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

        public async Task<IActionResult> Init()
        {
            DataContext context = new DataContext();
            if (await context.Board.CountAsync() == 0)
            {
                Board board = new Board();
                board.Index = new Guid("80000000-1000-0000-0000-000000000001");
                board.Name = "默认板块";
                board.CreateDate = board.EditDate = DateTime.Now;
                board.Description = "论坛初始化创建的默认板块。";
                context.Board.Add(board);

                Entity.Forum forum = new Entity.Forum();
                forum.Index = new Guid("80000000-1000-0000-0000-000000000002");
                forum.Board = board;
                forum.Name = "默认论坛";
                forum.CreateDate = board.EditDate = DateTime.Now;
                forum.Description = "论坛初始化创建的默认论坛。";
                context.Forum.Add(forum);

                Member admin = new Member();
                admin.Index = new Guid("80000000-1000-0000-0000-000000000003");
                admin.Username = "admin";
                admin.SetPassword("admin");
                admin.CreateDate = board.EditDate = DateTime.Now;
                context.Member.Add(admin);

                await context.SaveChangesAsync();
            }
            return RedirectToAction("SignIn");
        }

    }
}