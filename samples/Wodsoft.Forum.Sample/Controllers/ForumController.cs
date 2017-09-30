using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wodsoft.Forum.Mvc;
using Wodsoft.Forum.Sample.Entity;

namespace Wodsoft.Forum.Sample.Controllers
{
    public class ForumController : ForumControllerBase
    {
        [Route("")]
        [Route("Board/{id}")]
        public override Task<IActionResult> Board()
        {
            return base.Board();
        }

        [Route("Forum/{id}")]
        public override Task<IActionResult> Forum()
        {
            return base.Forum();
        }

        [Route("Thread/{id}")]
        public override Task<IActionResult> Thread()
        {
            return base.Thread();
        }
        
        [Route("Thread/Create/{id}")]
        public override Task<IActionResult> ThreadCreate()
        {
            return base.ThreadCreate();
        }

        [Route("Thread/Edit/{id}")]
        public override Task<IActionResult> ThreadEdit()
        {
            return base.ThreadEdit();
        }

        [Route("Thread/Update")]
        public override Task<IActionResult> ThreadUpdate()
        {
            return base.ThreadUpdate();
        }

        [Route("Thread/Delete")]
        public override Task<IActionResult> ThreadDelete()
        {
            return base.ThreadDelete();
        }

        [Route("Post/Create/{id}")]
        public override Task<IActionResult> PostCreate()
        {
            return base.PostCreate();
        }

        [Route("Post/Edit/{id}")]
        public override Task<IActionResult> PostEdit()
        {
            return base.PostEdit();
        }

        [Route("Post/Update")]
        public override Task<IActionResult> PostUpdate()
        {
            return base.PostUpdate();
        }

        [Route("Post/Delete")]
        public override Task<IActionResult> PostDelete()
        {
            return base.PostDelete();
        }
    }
}