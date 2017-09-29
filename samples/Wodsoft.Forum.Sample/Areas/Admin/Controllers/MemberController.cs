using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wodsoft.ComBoost.Mvc;
using Wodsoft.Forum.Sample.Entity;

namespace Wodsoft.Forum.Sample.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : EntityController<Member>
    {

    }
}
