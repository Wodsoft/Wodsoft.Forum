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

        [Route("Thread/Reply/{id}")]
        public override Task<IActionResult> PostCreate()
        {
            return base.PostCreate();
        }
    }
}