using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Mvc;

namespace Wodsoft.Forum.Sample.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ComBoostAuthorize]
    public class ForumController : EntityController<Entity.Forum>
    {
        public async Task<IActionResult> ImageToProperty(string path)
        {
            var storage = HttpContext.RequestServices.GetRequiredService<IStorageProvider>().GetStorage();
            var file = await storage.GetAsync(path);
            if (file == null)
                return NotFound();
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            string mimeType;
            if (!provider.TryGetContentType(path, out mimeType))
                mimeType = "application/octet-stream";
            return File(file, mimeType);
        }
    }
}
