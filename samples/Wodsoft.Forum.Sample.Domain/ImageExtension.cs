using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum.Sample.Domain
{
    public class ImageExtension<T> : DomainExtension
        where T : class, IEntity, new()
    {
        public override void OnInitialize(IServiceProvider serviceProvider, IDomainService domainService)
        {
            var domain = (EntityDomainService<T>)domainService;
            domain.EntityPropertyUpdate += Domain_EntityPropertyUpdate;
        }

        private async Task Domain_EntityPropertyUpdate(IDomainExecutionContext context, EntityPropertyUpdateEventArgs<T> e)
        {
            if (e.Property.Type == System.ComponentModel.DataAnnotations.CustomDataType.Image)
            {
                e.IsHandled = true;
                var storage = context.DomainContext.GetRequiredService<IStorageProvider>().GetStorage();
                var file = (ISelectedFile)e.Value;
                if (file == null)
                    return;
                var path = await storage.PutAsync(file.Stream, file.Filename);
                e.Property.SetValue(e.Entity, path);
            }
        }
    }
}
