using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data;
using Wodsoft.ComBoost.Data.Entity;
using Wodsoft.ComBoost.Mvc;
using Wodsoft.ComBoost.Security;

namespace Wodsoft.Forum.Mvc
{
    public class ForumControllerBase : DomainController
    {
        protected virtual ControllerDomainContext GetBoardDomainContext()
        {
            return CreateDomainContext();
        }
        public virtual async Task<IActionResult> Board()
        {
            var context = GetBoardDomainContext();
            {
                var domain = DomainProvider.GetService<ForumDomainService>();
                var result = await domain.ExecuteListForums(context);
                return View(result);
            }
        }

        protected virtual ControllerDomainContext GetForumDomainContext()
        {
            return CreateDomainContext();
        }
        public virtual async Task<IActionResult> Forum()
        {
            var context = GetForumDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var forum = await domain.ExecuteGetForum(context);
                ViewBag.Forum = forum;
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
            var result = await domain.ExecuteListThreads(context);
            return View(result);
        }

        protected virtual ControllerDomainContext GetThreadDomainContext()
        {
            return CreateDomainContext();
        }
        public virtual async Task<IActionResult> Thread()
        {
            var context = GetThreadDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var thread = await domain.ExecuteGetThread(context);
                ViewBag.Thread = thread;
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
            var result = await domain.ExecuteListPosts(context);
            return View(result);
        }

        protected virtual ControllerDomainContext GetThreadCreateDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> ThreadCreate()
        {
            var context = GetThreadCreateDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var thread = await domain.ExecuteCreateThread(context);
                return View(thread);
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else if (ex.InnerException is ArgumentException || ex.InnerException is ArgumentNullException || ex.InnerException is ArgumentOutOfRangeException)
                    return BadRequest();
                else if (ex.InnerException is UnauthorizedAccessException)
                    return Unauthorized();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }

        protected virtual ControllerDomainContext GetThreadEditDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> ThreadEdit()
        {
            var context = GetThreadEditDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var post = await domain.ExecuteEditThread(context);
                return View(post);
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else if (ex.InnerException is ArgumentException || ex.InnerException is ArgumentNullException || ex.InnerException is ArgumentOutOfRangeException)
                    return BadRequest();
                else if (ex.InnerException is UnauthorizedAccessException)
                    return Unauthorized();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }

        protected virtual ControllerDomainContext GetThreadUpdateDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> ThreadUpdate()
        {
            var context = GetThreadUpdateDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var thread = await domain.ExecuteUpdateThread(context);
                return Json(new
                {
                    ThreadId = thread.Index
                });
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else if (ex.InnerException is ArgumentException || ex.InnerException is ArgumentNullException || ex.InnerException is ArgumentOutOfRangeException)
                    return BadRequest(ex.InnerException.Message);
                else if (ex.InnerException is UnauthorizedAccessException)
                    return Unauthorized();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }

        protected virtual ControllerDomainContext GetThreadDeleteDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> ThreadDelete()
        {
            var context = GetThreadUpdateDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                await domain.ExecuteDeleteThread(context);
                return Ok();
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else if (ex.InnerException is ArgumentException || ex.InnerException is ArgumentNullException || ex.InnerException is ArgumentOutOfRangeException)
                    return BadRequest(ex.InnerException.Message);
                else if (ex.InnerException is UnauthorizedAccessException)
                    return Unauthorized();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }

        protected virtual ControllerDomainContext GetPostCreateDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> PostCreate()
        {
            var context = GetPostCreateDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var post = await domain.ExecuteCreatePost(context);
                return View(post);
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound(ex.InnerException.Message);
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }

        protected virtual ControllerDomainContext GetPostEditDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> PostEdit()
        {
            var context = GetPostEditDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var post = await domain.ExecuteEditPost(context);
                return View(post);
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound(ex.InnerException.Message);
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }

        protected virtual ControllerDomainContext GetPostUpdateDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> PostUpdate()
        {
            var context = GetPostUpdateDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                var post = await domain.ExecuteUpdatePost(context);
                return Ok();
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else if (ex.InnerException is ArgumentException || ex.InnerException is ArgumentNullException || ex.InnerException is ArgumentOutOfRangeException)
                    return BadRequest(ex.InnerException.Message);
                else if (ex.InnerException is UnauthorizedAccessException)
                    return Unauthorized();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }

        protected virtual ControllerDomainContext GetPostDeleteDomainContext()
        {
            return CreateDomainContext();
        }
        [ComBoostAuthorize]
        public virtual async Task<IActionResult> PostDelete()
        {
            var context = GetPostDeleteDomainContext();
            var domain = DomainProvider.GetService<ForumDomainService>();
            try
            {
                await domain.ExecuteDeletePost(context);
                return Ok();
            }
            catch (DomainServiceException ex)
            {
                if (ex.InnerException is KeyNotFoundException)
                    return NotFound();
                else if (ex.InnerException is ArgumentException || ex.InnerException is ArgumentNullException || ex.InnerException is ArgumentOutOfRangeException)
                    return BadRequest(ex.InnerException.Message);
                else if (ex.InnerException is UnauthorizedAccessException)
                    return Unauthorized();
                else
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
            }
        }
    }
}
